   M S B A M L   `   `   `  ���� [  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  H DPolygonEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PolygonEditorPolygonEditor    PolygonEditor.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     *сclr-namespace:PolygonEditor     X TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  -   $��windowPolygonEditor��  � Title$  Polygon Editor�� � Icon$ images/Simple.ico��$	��535��$	��864��$	��535��$	��864�� � WindowStartupLocation$ CenterScreen=� � 	Resources  �� MergedDictionaries �� Source��  �� $ Dictionary.xaml?�%     J� InputBindings �� 
 �� Key$ Escape�� �� Command �� *
PressESC+�� $	 End�� �� *
PressEnd+	 J� CommandBindings	 )  #System.Windows.Input.CommandBinding  -   
   Command$$
 ApplicationCommands.Properties��  -   $
 ApplicationCommands.Print��  -   $#
 ApplicationCommands.SelectAll��  -   $
 ApplicationCommands.Help��.��� -    �Name$ _EditorGrid��.+�o�  J� RenderTransformOrigin$ 
0.34,0.538&�$��20��$	��Top=�.@�m� -    m�Name$
 FILE��$
��FILE��$��2,0,0,0q�.@�m� -   $
 LOAD��$��Load...��.@�m� -	   $ LoadFormFile��$��	from File�� � ToolTip$; 5Load Shape/ShapeCollection/PolygonOperation from File�� m� Command �� *LoadInstanceFromFile+m� -
   $ LoadFromDataBase��$��from DataBase��$? 9Load Shape/ShapeCollection/PolygonOperation from DataBase�� �� *LoadInstanceFromDataBase+m� $��Save(Selected)...��{��� *SelectedShapeCount+   NumericToBooleanConverter �� 	Converter# [  .@�m� $��to File��$( "Save Shape/ShapeCollection to File�� �� *SaveShapesToFile+m� $��to DataBase��$, &Save Shape/ShapeCollection to DataBase�� �� *SaveShapesToDataBase+m� $��Save All...��{��� *
ShapeCount+# [  .@�m� $��to File��$( "Save Shape/ShapeCollection to File�� m� CommandParameter$	 All�� �� *SaveShapesToFile+m� $��to DataBase��$, &Save Shape/ShapeCollection to DataBase��$	 All�� �� *SaveShapesToDataBase+�� m� $��Properties...��$( "Configuring application properties��$ 
Properties���� m� $��Print...��$$ Print all Shapes on the Canvas��$ Print�� m� Icon �� $��images\print.png���� m� -   $
��Exit�� m� InputGestureText$ Alt+F4�� �� $��images\close.png��m� $
��EDIT��.@�m� $��Mode: Creat New Shape�� �� *SetDrawNewMode+m� $��
Mode: Edit�� �� *SetEditMode+{��� *
ShapeCount+# [  m� $��Mode: Selection�� �� *SetSelectionMode+{��� *
ShapeCount+# [  m� $��Mode: PolygonOperations�� �� *SetPolygonOperationsMode+{��� *PolygonsCount+# [   �� ConverterParameter$ 2���� m� $��
Select All��$ 	SelectAll��{��� *
ShapeCount+# [  m� -   $��Unselect All��{��� *SelectedShapeCount+# [  �� m� -   $��Clear Canvas��{��� *
ShapeCount+# [  m� $
��HELP��$��Right=�.@�m� $��Help Topics��$
 Help�� �� $��images\help.png���� m� -   $��About...��$ Application Info��`�  `� Orientation$ 
Horizontal=�$��0,20,0,0q�$��34��$	��Top=� `� 
Background ��  �� ImageSource$ images\bg_46.jpg�� j� Viewport$ 0,0,256,256� j� ViewportUnits$ Absolute=� j� TileMode$
 Tile=�.�c� -   $��ToolBar_DrawProperty��$��33,0,0,0q�y���  �� ElementName$# RButton_modePolygonOperations�� �� Path$ 	IsChecked� ' #BooleanToVisibilityConverterInverse# [ .@�	� -   $��RButton_drawPoint�� 	� 	GroupName$ 	ShapeType��  RButton_drawPoint#��[ $��1q�$ 
Draw Point��{��� $ RButton_modeDrawNew��$ 	IsChecked� d� 	IsChecked �� *	ShapeType+  ShapeTypeToBooleanConverter# [ $ Point��	� -   $��RButton_drawLine��$ 	ShapeType��  RButton_drawLine#��[ $��1q�$ 	Draw Line��{��� $ RButton_modeDrawNew��$ 	IsChecked� �� *	ShapeType+# [ $
 Line��	� -   $��RButton_drawPolyLine��$ 	ShapeType��  RButton_drawPolyline#��[ $��1q�$ Draw PolyLine��{��� $ RButton_modeDrawNew��$ 	IsChecked� �� *	ShapeType+# [ $ PolyLine��	� -   $��RButton_drawPolygon��$ 	ShapeType��  RButton_drawPolygon#��[ $��1q�$ Draw Polygon��{��� $ RButton_modeDrawNew��$ 	IsChecked� �� *	ShapeType+# [ $ Polygon���� �� $��StrokeWeight:��$��1,8,1,1q�  Tool_textBlock#��[ {�`�   BooleanConverterOR `� 	Converter# [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked��� -   $ ��Selector_ShapeStrokeWeight��$��2q� 	 Tool_weightPicker#��[	 |�.  #��� ��  �� SelectedIndex  �� *ShapeStrokeWidthForCombo+{�`� # [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked�.@��� ! �� 
IsSelected! . .���� 
���   �	" �� X1$" 0��	# �� X2$# 40��	$ �� Y1$$ 10��	% �� Y2$% 10��$��20��$��1��& �� EdgeMode$& Aliased=��� .���� 
���   �$" 0��$# 40��$$ 10��$% 10��$��20��$��2��$& Aliased=��� .���� 
���   �$" 0��$# 40��$$ 10��$% 10��$��20��$��3��$& Aliased=��� .���� 
���   �$" 0��$# 40��$$ 10��$% 10��$��20��$��4��$& Aliased=��� $��StrokeColor:��$��1,8,1,1q�#��[ {�`� # [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked�#  Xceed.Wpf.Toolkit.ColorPicker  -   '  Name$' Selector_ShapeStrokeColor��$��2q�$��60�� 
 Tool_сolorPicker#��[
 |�.  (   SelectedColor( �� *ShapeStrokeColor+{�`� # [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked��� -   ) ��Name$) TextBlock_FillColor��$��
FillColor:��$��5,8,1,1q�#��[ y��� $ RButton_drawPolygon��$ 	IsChecked�   BooleanToVisibilityConverter# [ {�`� # [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked�  -   $' Selector_ShapeFillColor��$��2q�$��60��#��[
 |�.  y��� $ RButton_drawPolygon��$ 	IsChecked�# [ ( �� *ShapeFillColor+{�`� # [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked�c� -   $��ToolBar_PolygonOperations��$��33,0,-164,0q�y��� $# RButton_modePolygonOperations��$ 	IsChecked�# [ .@��� $	��5,1q�.��OPERATION :�� -   $��Selector_OperationType��$	��120��$��25��$��1q�#��� ������ *PolygonBinOptions+* �� SelectedItem* �� *SelectedPolygonBinOption++ �� Mode$+ TwoWay=��� -   $��Button_RunOperation��$	��Run��$	��5,2q�$��50��  Botton_Botton#��[ ���� *RunOperation+�� -   $��Button_DeleteResults��$��DeleteResults��$	��5,2q�#��[ ���� *DeleteResults+d� -   $��ToggleButton_SetClippers��$��SetClippers��$	��5,1q�  Botton_ToggleBotton#��[  �� *SetClippersOn+�� -   $��Button_UndoClippers��$��UndoClippers��$	��5,1q�#��[ ���� *UndoClippers+�� �� $��5,1,0,1q�.��SHOW :�� -   $��CheckBox_ShowSubjects��$��Subjects��$	��5,1q�$��50��  Botton_CheckBox#��[  �� *ShowSubjects+$+ TwoWay=��� -   $��CheckBox_ShowClippers��$��Clippers��$	��5,1q�$��50��#��[  �� *ShowClippers+$+ TwoWay=��� -    $��CheckBox_ShowResults��$��Results��$��5,1,10,1q�$��50��#��[  �� *ShowResults+$+ TwoWay=�`� $ Vertical=�$
��Left=�$��	0,54,0,27q�$��36�� �� $ images\bg_46.jpg��$ 0,0,256,256�$ Absolute=�$
 Tile=�.�c� -!   $��ToolBar_EditorMode��$��0,-2,0,0q�.@��� -"   $��Button_SaveSelectedShapes��  Button_Save#��[ $��1q�$ Save the Selected Shapes��{��� *SelectedShapeCount+# [  ���� *SaveShapesToFile+�� -#   $��Button_SaveAll��  Button_Save_All#��[ $��1q�$ Save All the Shapes��$	��All��{��� *
ShapeCount+# [  ���� *SaveShapesToFile+�� -$   $��Button_Print��  Button_Print#��[ $��1q�$$ Print all Shapes on the Canvas��$��Print���� $	��0,5q��� -%   $��Button_Load��  Button_Load#��[ $��1q�$ Load Shape(s)������ *LoadInstanceFromFile+	� -&   $��RButton_modeDrawNew��$
 Mode��  RButton_modeNew#��[ $��1q�$ Draw New Shape�� �� *
EditorMode+  ModeToBooleanConverter# [ $ DrawNew������ *SetDrawNewMode+	� -'   $��RButton_modeEdit��$
 Mode��  RButton_modeEdit#��[ $��1q�$ 	Edit Mode�� �� *
EditorMode+# [ $
 Edit��{��� *
ShapeCount+# [  ���� *SetEditMode+	� -(   $��RButton_modeSelection��$
 Mode��  RButton_modeSelect#��[ $��1q�$ Selection Mode�� �� *
EditorMode+# [ $ 	Selection��{��� *
ShapeCount+# [  ���� *SetSelectionMode+�� $��0,5,0,7q�	� -)   $#��RButton_modePolygonOperations��$
 Mode�� ! RButton_modePolygonOperations#��[ $��1q�$ PolygonOperations Mode�� �� *
EditorMode+# [ $ PolygonOperations��{��� *PolygonsCount+# [  $ 2������ *SetPolygonOperationsMode+�� $��0,7,0,5q��� -*   $!��Button_DeleteSelectedShapes��  
Button_Del#��[ $��1q�$  Delete the Selected Shapes��{��� *SelectedShapeCount+# [  �� -+   $��Button_CanvasClearing��  
Button_Clr#��[ $��1q�$) #Clear Canvas(Delete All the Shapes)��{��� *
ShapeCount+# [  � -,   $ ParametersPanel��$��	0,54,0,27q�$��Right=�$	��119��{�`� # [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked����� $ images\bg_46.jpg��$ 0,0,256,256�$ Absolute=�$
 Tile=�.+��� --   $��
PointPanel��  StackPanel_Coords#��[ ���� *ActualWidth+$ ParametersPanel������ *ActualHeight+$ ParametersPanel��{��� *IsMouseDraws+  BooleanConverterNOT# [ y��� # [ $ RButton_drawPoint��$ 	IsChecked�.+��� $��10(�$
��Bold"�.��	Geometry:�� $��15(�$
��Bold"�$��Right=�$��0,0,10,0q�.��Point�� $��12(�$
��Bold"�$��0,10,0,0q�.��Coords(x,y):�� -.   $��PointCoords��$��60��$��Right=�$��
10,0,10,10q�, �� 
AcceptsTab, .  �� -/     GlassButton#���  $��InsertShape��$ Insert a new shape��$��5q�$��90��$��30��y��� # [ $ RButton_modeDrawNew��$ 	IsChecked��� -0   #���  $��UpdateCoords��$2 ,Update the coordinates of the selected shape��$��5q�$��90��$��30��y��� # [ $ RButton_modeEdit��$ 	IsChecked�{��� *IsSelectedShapeForEdit+�� -1   $��	LinePanel��#��[ ���� *ActualWidth+$ ParametersPanel������ *ActualHeight+$ ParametersPanel��{��� *IsMouseDraws+# [ y��� # [ $ RButton_drawLine��$ 	IsChecked�.+��� $��10(�$
��Bold"�.��	Geometry:�� $��15(�$
��Bold"�$��Right=�$��0,0,10,0q�.��Line�� $��12(�$
��Bold"�$��0,10,0,0q�.��FirstPoint(x,y):�� -2   $��
FirstPoint��$��60��$��Right=�$��
10,0,10,10q�, .  �� $��12(�$
��Bold"�$��0q�.��SecondPoint(x,y):�� -3   $��SecondPoint��$��60��$��Right=�$��
10,0,10,10q�, .  �� -4   #���  $��InsertShape��$ Insert a new shape��$��5q�$��90��$��30��y��� # [ $ RButton_modeDrawNew��$ 	IsChecked�{��� # [ $ IsMouseDraws��� -5   #���  $��UpdateCoords��$2 ,Update the coordinates of the selected shape��$��5q�$��90��$��30��y��� # [ $ RButton_modeEdit��$ 	IsChecked�{��� *IsSelectedShapeForEdit+�� -6   $��PolyLinePanel��}�.   Canvas_Coords#��[ ���� *ActualWidth+$ ParametersPanel������ *ActualHeight+$ ParametersPanel��{��� *IsMouseDraws+# [ y��� # [ $ RButton_drawPolyLine��$ 	IsChecked�.+��� $��10(�$
��Bold"�.��	Geometry:�� $��15(�$
��Bold"�$��Right=�- �� Left$- 44��
. �� Top$. 12��.��
PolyLine�� $��12(�$
��Bold"�$. 45��.��
Point_1:�� -7   $��LPoint_1��$��60��, .  $- 56��$. 45���� $��12(�$
��Bold"�$. 65��.��
Point_2:�� -8   $��LPoint_2��$��60��, .  $- 56��$. 65���� $��12(�$
��Bold"�$. 85��.��
Point_3:�� -9   $��LPoint_3��$��60��, .  $- 56��$. 85���� -:   #���  $��50��$��20��$ Del Point from Shape��$- 64��$	. 110��$��- Point��/ J� Opacity$	/ 0.4Q�y��� # [ $ RButton_modeSelection��$ 	IsChecked�{��� # [ $ IsMouseDraws��� -;   #���  $��50��$��20��$ Add Point to Shape��$- 6��$	. 110��$��+ Point��y��� # [ $ RButton_modeSelection��$ 	IsChecked�{��� # [ $ IsMouseDraws��� -<   $��UpdateCoords��$2 ,Update the coordinates of the selected shape��#���  $- 14��$	. 140��$��90��$��30��y��� # [ $ RButton_modeEdit��$ 	IsChecked�{��� *IsSelectedShapeForEdit+�� -=   $��InsertShape��$ Insert a new shape��#���  $- 14��$	. 140��$��90��$��30��y��� # [ $ RButton_modeDrawNew��$ 	IsChecked�{��� # [ $ IsMouseDraws��� ->   $��PolygonPanel��}�. #��[ ���� *ActualWidth+$ ParametersPanel������ *ActualHeight+$ ParametersPanel��{��� *
IsPrompt+# [ $+ TwoWay=�y��� # [ $ RButton_drawPolygon��$ 	IsChecked�.+��� $��10(�$
��Bold"�.��	Geometry:�� $��15(�$
��Bold"�$��Right=�$- 44��$. 12��.��
Polygone�� $��12(�$
��Bold"�$. 45��.��
Point_1:�� -?   $��PPoint_1��$��60��, .  $- 56��$. 45���� $��12(�$
��Bold"�$. 65��.��
Point_2:�� -@   $��PPoint_2��$��60��, .  $- 56��$. 65���� $��12(�$
��Bold"�$. 85��.��
Point_3:�� -A   $��PPoint_3��$��60��, .  $- 56��$. 85���� -B   #���  $��50��$��20��$ Del Point from Shape��$- 64��$	. 110��$��- Point��$	/ 0.4Q�{��� # [ $ IsMouseDraws�y�`�  " BooleanToVisibilityConverterOR# [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked��� -C   #���  $��50��$��20��$ Add Point to Shape��$- 6��$	. 110��$��+ Point��{��� # [ $ IsMouseDraws�y�`� # [ .1��� $ RButton_modeDrawNew��$ 	IsChecked��� $ RButton_modeEdit��$ 	IsChecked��� -D   $��UpdateCoords��$2 ,Update the coordinates of the selected shape��#���  $- 14��$	. 140��$��90��$��30��y��� # [ $ RButton_modeEdit��$ 	IsChecked�{��� *IsSelectedShapeForEdit+�� -E   $��InsertShape��$ Insert a new shape��#���  $- 14��$	. 140��$��90��$��30��y��� # [ $ RButton_modeDrawNew��$ 	IsChecked�{��� # [ $ IsMouseDraws��� $��26��$��Bottom=�$��Stretch=�.@��� 0 �� Orientation$0 
Horizontal=�.+��� $��CurrentMode:���� $��2,0,10,0q�
����  ����� *
EditorMode+�� $��
ShapeType:���� $��2,0,10,0q�
����  ����� *	ShapeType+�� $��CursorPosition:���� $��2,0,10,0q�
����  ����� *	CursorPos+�� -F   1 ��Name$1 
EditorPane��$��37,55,120,27q�$��Stretch=�$��Stretch=�}�. |�. 2 +� IsFocusScope2 . .+��� -G   $1 BackgroundPane��$- 0��$. 0������ *ActualWidth+$ 
EditorPane������ *ActualHeight+$ 
EditorPane����!� -H   	LogoBrush��$ 0,0,0.335,0.2�$
 Tile=�3 �� 	Transform3 �� 4 �� Angle$4 0Q�5 �� CenterX$	5 400Q�6 �� CenterY$	6 250Q�7 !� Visual7 �� 
������ ._��� -I   $) geoText��$ �� Welcome to PolygonEditor ��$��Comic Sans MS)�
�������.+��� -J   8 ��Name$8 	RectBrush��$
��Left=����� *ActualWidth+$ 
EditorPane������ *ActualHeight+$ 
EditorPane����L� -K   	GridBrush��$ 	0,0,10,10�$ Absolute=�$
 Tile=�9 �� Opacity$9 0Q�: L� Drawing: I� .��� ; � Brush; �#CCCCFF< � Geometry< �� = �� Rect$= 0,0 10,1�� ; �#CCCCFF< �� $= 0,0 1,10�> � Triggers> :� ? :� RoutedEvent$? Loaded��.[��� .���� .��W� @ �� 
TargetName$@ 	LogoBrush��A �� TargetProperty$5A /(VisualBrush.Transform).(RotateTransform.Angle)�B W� From$B 0X�	C W� To$	C -30X�D g� 	BeginTime$D 0:0:1X�E g� Duration$E 0:0:2D�F g� AutoReverseF . W� $@ geoText��$A Opacity�$B 1X�$C 0X�$D 0:0:1X�$E 0:0:2D�W� $@ 	GridBrush��$A Opacity�$B 0X�$C 1X�$D 0:0:1X�$E 0:0:2D��� -L   $1 DrawingPane��$- 0��$. 0������ *ActualWidth+$ 
EditorPane������ *ActualHeight+$ 
EditorPane���� -M   $1 ControlPane��$- 0��$. 0��|�. 2 . ���� *ActualWidth+$ 
EditorPane������ *ActualHeight+$ 
EditorPane���� -N   $��Prompt��$Y��SPress "End" to finish drawing the shape or press "Еsc" to delete the drawn segment��$��29��$	��442��$	- 252��$. -7��  Label_Prompt#��[ $��11(�$
��Bold"�
����  �{�`�    BooleanConverterAND# [  .1��� $ IsPrompt��� $ RButton_modeDrawNew��$ 	IsChecked�