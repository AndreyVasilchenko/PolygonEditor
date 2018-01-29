using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Attributes;

namespace PolygonEditor
{
    public class shapeGroup
    {
        public String groupName { get; set; }
        public List<ShapeParams> Shapes;
    }
    public class nameInfo
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public string Name { get; set; }
    }
 
    public class My_MongoDB
    {
        private bool is_open = false;
        public bool isOpen { get { return is_open; } }  //  Флаг - база данных открыта для запросов получения данных

        public MongoClient client = null;               //  Ссылка на подключение к серверу БД
        public IMongoDatabase db = null;                //  Ссылка на саму БД

        private bool selfrun_mongod = false;            //  Флаг - сервер БД запущен при открытии БД
        private Process mongodProcess = null;           //  Ссылка на процесс запустивший сервер БД

        private bool remoteUsed;                        //  Какой сервер БД использовать? true - удаленный, false - локальный
        private string host;                            //  Адрес хоста
        private string port;                            //  Номер порта на хосте
        private string dbName;                          //  Название БД
        private string userLogin;                       //  Логин доступа к БД
        private string userPassword;                    //  Пароль доступа к БД

        public void OpenDB(CProperties save_property, string _dbName)
        {
            BsonClassMap.RegisterClassMap<namedParameter>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(cl => cl.ParamName);
                cm.MapMember(cl => cl.ParamValue).SetSerializer(new myObjectSerializer());
                cm.SetIgnoreExtraElements(true);

            });

            BsonClassMap.RegisterClassMap<ShapeParams>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(cl => cl.ShapeName);
                cm.MapMember(cl => cl.ShapeType);
                cm.MapMember(cl => cl.StrokeWeight);
                cm.MapMember(cl => cl.StrokeColor);
                cm.MapMember(cl => cl.FillColor);
                cm.MapMember(cl => cl.GeometricParams);
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<shapeGroup>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(cl => cl.groupName);
                cm.MapMember(cl => cl.Shapes);
                cm.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<PolygonOperation>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(cl => cl.OperationName);
                cm.MapMember(cl => cl.SubjectGeometry);
                cm.MapMember(cl => cl.ClipperGeometry);
                cm.MapMember(cl => cl.OperationType).SetSerializer(new EnumSerializer<BooleanOperations>(BsonType.String)); ;
                cm.MapMember(cl => cl.ResultGeometry);
                cm.SetIgnoreExtraElements(true);
            });

            remoteUsed = save_property.RemoteUsed;
            dbName = _dbName;
            userLogin = save_property.UserLogin.Trim();
            userPassword = save_property.UserPassword.Trim();

            selfrun_mongod = false;
            mongodProcess = null;
            is_open = false;

            if (remoteUsed)
            {
                host = save_property.RemoteHost.Trim();
                port = save_property.RemotePort.Trim();
            }
            else
            {
                host = save_property.LocalHost.Trim();
                port = save_property.LocalPort.Trim();
            }
            
            try
            {
                if (userLogin.Length > 0)
                {
                    client = new MongoClient(new MongoClientSettings
                    {
                        Credential = MongoCredential.CreateCredential(dbName, userLogin, userPassword),
                        Server = new MongoServerAddress(host, Convert.ToInt32(port))
                    });
                }
                else
                {
                    client = new MongoClient(new MongoClientSettings
                    {
                        Server = new MongoServerAddress(host, Convert.ToInt32(port))
                    });
                }

                db = client.GetDatabase(dbName);
                is_open = true;

                if (!remoteUsed) 
                {
                    bool ret_val = db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(500);
                    if (!ret_val)
                    {
                        is_open = false;                                                                     //Пинга к необходимому порту нет
                    }
                }
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException(ex.Message);
            }


      /*******----  Этот блок доделать, если получится сконфигурировать портативный сервер MongoDB ------------------*******
            
                Process[] procs = Process.GetProcessesByName("mongod");     
                if (procs != null && procs.Count() > 0)                                                 // Если процесс с запущенным Сервером БД существует
                {
                    client = new MongoClient("mongodb://"+host+":" + port);                             //      пытаемся подключиться к 
                    db = client.GetDatabase(dbName);
                    bool ret_val = db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(500);
                    if (!ret_val)                                                                       
                    {
                        selfrun_mongod = true;                                                              //сервер на необходимый порт не пингуется
                    }
                    else
                    {
                        is_open = true;                                                                     //Пинг к необходимому порту есть
                    }
                }
                else
                    selfrun_mongod = true;

                if (selfrun_mongod)
                {
                    //Console.Write("Сервер 'MongoDB' НЕ запущен! Запускаю....\n");
                    ProcessStartInfo start = new ProcessStartInfo();
                    string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\MongoDB\";
                    start.FileName = dir + @"bin\mongod.exe";
                    start.Arguments = "--dbpath \"" + dir + "data\" --logpath \"" + dir + "log\\log\" --port "+port;
                    start.WindowStyle = ProcessWindowStyle.Hidden;
                    //start.UseShellExecute = false;
                    try
                    {
                        mongodProcess = Process.Start(start);
                    }
                    catch (Exception ex)
                    {
                        mongodProcess = null;
                        throw new System.ApplicationException("Запустить сервер 'MongoDB' не удалось.../n" + ex.Message);
                    }
                    //else
                        //Console.Write("Cервер 'MongoDB' загружен.\n\n");

                    client = new MongoClient("mongodb://" + host + ":" + port);
                    db = client.GetDatabase(dbName);
                    is_open = true;
                }
       ******---------------------------------------------------------************/
           
        }

        public bool CheckPing(int msec)
        {
            return db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(msec);
        }

        public void CloseDB()
        {
            if (isOpen)
                if (selfrun_mongod)
                {
                    // Останавливаем СерверБД
                    var AdminDatabase = client.GetDatabase("admin");
                    AdminDatabase.RunCommandAsync<BsonDocument>(new JsonCommand<BsonDocument>("{shutdown: 1}"));

                    //Закрываем процесс в котором работал сервер
                    mongodProcess.Kill();
                }
        }

        public void Save(string collection_name, string record_name, object save_instance)
        {
            if (isOpen)
            {
                try
                {
                    switch (collection_name)
                    {
                        case "Shapes":
                            var shape_collection = db.GetCollection<ShapeParams>(collection_name);
                            ((ShapeParams)save_instance).ShapeName= record_name;
                            var newShape = (ShapeParams)save_instance;
                            shape_collection.InsertOne(newShape);
                            break;

                        case "ShapeGroups":
                            var group_collection = db.GetCollection<shapeGroup>(collection_name);
                            var newGroup = new shapeGroup { groupName = record_name, Shapes = (List<ShapeParams>)save_instance };
                            group_collection.InsertOne(newGroup);
                            break;

                        case "PolygonOperations":
                            var op_collection = db.GetCollection<PolygonOperation>(collection_name);
                            var newOperation = (PolygonOperation)save_instance;
                            if (newOperation.OperationName == null)
                                newOperation.OperationName = record_name;
                            op_collection.InsertOne(newOperation);
                            break;

                        default:
                            MessageBox.Show("Unsuccessful attempt to save the unknown data structure !!!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new System.ApplicationException(ex.Message);
                }
            }
        }

        public List<nameInfo> GetNameList(string collection_name)
        {
            List<nameInfo> str_list = null;

            if (isOpen)
            {
                try
                {
                    switch (collection_name)
                    {
                        case "Shapes":
                            var shape_collection = db.GetCollection<ShapeParams>(collection_name);
                            var projection = Builders<ShapeParams>.Projection/*.Exclude("_id")*/.Include("ShapeName");
                            var list = shape_collection.Find(new BsonDocument()).Project(projection).ToList();
                            if (list != null && list.Count > 0)
                            {
                                str_list = new List<nameInfo>();
                                foreach (var shape in list)
                                {
                                    str_list.Add(new nameInfo() { Id = (BsonObjectId)shape["_id"], Name = (string)shape["ShapeName"] });
                                }
                            }
                            break;

                        case "ShapeGroups":
                            var group_collection = db.GetCollection<shapeGroup>(collection_name);
                            var projection_g = Builders<shapeGroup>.Projection/*.Exclude("_id")*/.Include("groupName");
                            var list_g = group_collection.Find(new BsonDocument()).Project(projection_g).ToList();
                            if (list_g != null && list_g.Count > 0)
                            {
                                str_list = new List<nameInfo>();
                                foreach (var group in list_g)
                                {
                                    str_list.Add(new nameInfo() { Id = (BsonObjectId)group["_id"], Name = (string)group["groupName"] });
                                    //str_list.Add((string)group["groupName"]);
                                }
                            }
                            break;

                        case "PolygonOperations":
                            var op_collection = db.GetCollection<PolygonOperation>(collection_name);
                            var projection_o = Builders<PolygonOperation>.Projection/*.Exclude("_id")*/.Include("OperationName");
                            var list_o = op_collection.Find(new BsonDocument()).Project(projection_o).ToList();
                            if (list_o != null && list_o.Count > 0)
                            {
                                str_list = new List<nameInfo>();
                                foreach (var operation in list_o)
                                {
                                    str_list.Add(new nameInfo() { Id = (BsonObjectId)operation["_id"], Name = (string)operation["OperationName"] });
                                    //str_list.Add((string)operation["OperationName"]);
                                }
                            }
                            break;

                        default:
                            MessageBox.Show("Неудачная попытка чтения из неизвестной структуры данных!!!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new System.ApplicationException(ex.Message);
                }
            }
            return str_list;
        }

        public object GetInstanceByID(string collection_name, BsonObjectId instance_id)
        {
            if (isOpen)
            {
                try
                {
                    switch (collection_name)
                    {
                        case "Shapes":
                            var shape_collection = db.GetCollection<ShapeParams>(collection_name);
                            var filter_sh = Builders<ShapeParams>.Filter.Eq("_id", instance_id);
                            var shape_list = shape_collection.Find(filter_sh).ToList();
                            if (shape_list != null && shape_list.Count > 0)
                                return shape_list[0];
                            break;

                        case "ShapeGroups":
                            var group_collection = db.GetCollection<shapeGroup>(collection_name);
                            var filter_gr = Builders<shapeGroup>.Filter.Eq("_id", instance_id);
                            var group_list = group_collection.Find(filter_gr).ToList();
                            if (group_list != null && group_list.Count > 0)
                                return group_list[0].Shapes;
                            break;

                        case "PolygonOperations":
                            var op_collection = db.GetCollection<PolygonOperation>(collection_name);
                            var filter_op = Builders<PolygonOperation>.Filter.Eq("_id", instance_id);
                            var operation_list = op_collection.Find(filter_op).ToList();
                            if (operation_list != null && operation_list.Count > 0)
                                return operation_list[0];
                            break;

                        default:
                            MessageBox.Show("Неудачная попытка чтения из неизвестной структуры данных!!!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new System.ApplicationException(ex.Message);
                }
            }
            return null;
        }

        public bool DeleteInstanceById(string collection_name, BsonObjectId instance_id)
        {
            if (isOpen)
            {
                try
                {
                    switch (collection_name)
                    {
                        case "Shapes":
                            var shape_collection = db.GetCollection<ShapeParams>(collection_name);
                            var filter_sh = Builders<ShapeParams>.Filter.Eq("_id", instance_id);
                            var shape_list = shape_collection.Find(filter_sh).ToList();
                            if (shape_list != null && shape_list.Count == 1)
                            {
                                var result = shape_collection.DeleteOne(filter_sh);
                                if (result.DeletedCount == 1)
                                    return true;
                            }
                            break;

                        case "ShapeGroups":
                            var group_collection = db.GetCollection<shapeGroup>(collection_name);
                            var filter_gr = Builders<shapeGroup>.Filter.Eq("_id", instance_id);
                            var group_list = group_collection.Find(filter_gr).ToList();
                            if (group_list != null && group_list.Count == 1)
                            {
                                var result = group_collection.DeleteOne(filter_gr);
                                if (result.DeletedCount == 1)
                                    return true;
                            }
                            break;

                        case "PolygonOperations":
                            var op_collection = db.GetCollection<PolygonOperation>(collection_name);
                            var filter_op = Builders<PolygonOperation>.Filter.Eq("_id", instance_id);
                            var operation_list = op_collection.Find(filter_op).ToList();
                            if (operation_list != null && operation_list.Count == 1)
                            {
                                var result = op_collection.DeleteOne(filter_op);
                                if (result.DeletedCount == 1)
                                    return true;
                            }
                            break;

                        default:
                            MessageBox.Show("Unsuccessful attempt to delete the unknown data structure !!!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new System.ApplicationException(ex.Message);
                }
            }
            return false;
        }

        public bool RenameInstanceById(string collection_name, BsonObjectId instance_id, string instance_name)
        {
            if (isOpen)
            {
                try
                {
                    switch (collection_name)
                    {
                        case "Shapes":
                            var shape_collection = db.GetCollection<ShapeParams>(collection_name);
                            var filter_sh = Builders<ShapeParams>.Filter.Eq("_id", instance_id);
                            var shape_list = shape_collection.Find(filter_sh).ToList();
                            if (shape_list != null && shape_list.Count == 1)
                            {
                                var update = Builders<ShapeParams>.Update.Set("ShapeName", instance_name);
                                var result = shape_collection.UpdateOne(filter_sh,update);
                                if (result.ModifiedCount == 1)
                                    return true;
                            }
                            break;

                        case "ShapeGroups":
                            var group_collection = db.GetCollection<shapeGroup>(collection_name);
                            var filter_gr = Builders<shapeGroup>.Filter.Eq("_id", instance_id);
                            var group_list = group_collection.Find(filter_gr).ToList();
                            if (group_list != null && group_list.Count == 1)
                            {
                                var update = Builders<shapeGroup>.Update.Set("groupName", instance_name);
                                var result = group_collection.UpdateOne(filter_gr,update);
                                if (result.ModifiedCount == 1)
                                    return true;
                            }
                            break;

                        case "PolygonOperations":
                            var op_collection = db.GetCollection<PolygonOperation>(collection_name);
                            var filter_op = Builders<PolygonOperation>.Filter.Eq("_id", instance_id);
                            var operation_list = op_collection.Find(filter_op).ToList();
                            if (operation_list != null && operation_list.Count == 1)
                            {
                                var update = Builders<PolygonOperation>.Update.Set("OperationName", instance_name);
                                var result = op_collection.UpdateOne(filter_op, update);
                                if (result.ModifiedCount == 1)
                                    return true;
                            }
                            break;

                        default:
                            MessageBox.Show("Unsuccessful attempt to delete the unknown data structure !!!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new System.ApplicationException(ex.Message);
                }
            }
            return (false);
        }

        internal class myObjectSerializer : SerializerBase<object>
        {
            public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object obj)
            {
                context.Writer.WriteStartDocument();
                context.Writer.WriteName("_t");

                if (obj is Point)
                {
                    context.Writer.WriteString("Point");
                    context.Writer.WriteName("X");
                    context.Writer.WriteDouble(((Point)obj).X);
                    context.Writer.WriteName("Y");
                    context.Writer.WriteDouble(((Point)obj).Y);
                }
                else
                {
                    context.Writer.WriteString("Unknown");
                }
                context.Writer.WriteEndDocument();
            }

            public override object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                object obj = null;

                context.Reader.ReadStartDocument();
                string object_type = context.Reader.ReadString();
                switch (object_type)
                {
                    case "Point":
                        int cnt = 0;
                        Point point = new Point();
                        for (int i = 0; i < 3; i++)
                        {
                            string name = context.Reader.ReadName();
                            switch (name)
                            {
                                case "X":
                                case "x":
                                    point.X = (int)context.Reader.ReadDouble();
                                    cnt++;
                                    break;
                                case "Y":
                                case "y":
                                    point.Y = (int)context.Reader.ReadDouble();
                                    cnt++;
                                    break;
                            }
                            if (cnt > 1)
                            {
                                obj = point;
                                break;
                            }
                        }
                        /*point.X = (int)context.Reader.ReadDouble();
                        point.Y = (int)context.Reader.ReadDouble();*/
                        break;
                    default:
                        break;
                }

                context.Reader.ReadEndDocument();

                return obj;
            }
        }
        
    }
}