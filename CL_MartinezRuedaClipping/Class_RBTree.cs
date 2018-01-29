using System;
using System.Windows;
using System.Collections.Generic;


namespace CL_MartinezRuedaClipping
{
    public partial class MartinezRuedaClipping
    {
        internal delegate int? Comporator(SweepEvent a, SweepEvent b);

        internal delegate List<SweepEvent> Visit(SweepEvent key, SweepEvent val, List<SweepEvent> list);

        internal enum RBTreeNodeColor
        {
            RED   = 0,
            BLACK = 1
        }

        internal class RBNode
        {
            internal RBTreeNodeColor color;
            internal SweepEvent key;
            internal SweepEvent value;
            internal RBNode left;
            internal RBNode right;
            internal long count;

            internal RBNode(RBTreeNodeColor color, SweepEvent key, SweepEvent value, RBNode left, RBNode right, long count) 
            {
              this.color = color;
              this.key = key;
              this.value = value;
              this.left = left;
              this.right = right;
              this.count = count;
            }
            
            internal void recount()
            {
              this.count = 1 + (this.left != null ? this.left.count : 0) + (this.right != null ? this.right.count : 0);
            }

            internal RBNode cloneNode()
            {
                return new RBNode(this.color, this.key, this.value, this.left, this.right, this.count);
            }

            internal RBNode repaint(RBTreeNodeColor color) 
            {
              return new RBNode(color, this.key, this.value, this.left, this.right, this.count);
            }

            //public int CompareTo(SweepEvent compKey)
            //{
            //    return key.CompareTo(compKey);
            //}
            //public int CompareTo(SweepEvent compValue)
            //{
            //    return value.CompareTo(compValue);
            //}
        }


        internal class RedBlackTree
        {
            internal Comporator compare;

            internal RBNode root  { get; private set; }
            
            //Default comparison function
            //internal int? defaultCompare<T>(T a, T b)
            //{
            //    if ( a is long && b is long)
            //    {
            //        long a1 = (long)Convert.ChangeType(a, typeof(long));
            //        long b1 = (long)Convert.ChangeType(b, typeof(long));

            //        return ((a1 < b1) ? -1 : ((a1 > b1) ? 1 : 0));
            //    }
            //    return null;
            //}

            internal RedBlackTree(Comporator _compare, RBNode _root)
            {
                //if( _compare != null )
                    this.compare = _compare;
                //else
                //    this.compare = defaultCompare;
                
                this.root = _root;
            }

            //Build a tree
            internal RedBlackTree(Comporator _compare) 
            {
                //*if (_compare != null)
                    this.compare = _compare;
                ///*else
                //    this.compare = defaultCompare;

                this.root = null;
            }

//var proto = RedBlackTree.prototype

            internal List<SweepEvent> keys
            {
                get
                {
                    List<SweepEvent> key_list = new List<SweepEvent>();
                    ForEach(key_list, (key, value, list) => {
                        if(key != null)
                            list.Add(key);
                        return null;
                    });
                    return key_list;
                }
            }

            internal List<SweepEvent> values
            {
                get
                {
                    List<SweepEvent> val_List = new List<SweepEvent>();
                    ForEach(val_List, (key, value, list) => { 
                        if (value != null) 
                            list.Add(value);
                       
                        return null; 
                    });
                    return val_List;
                }
            }

            internal long length {//Returns the number of nodes in the tree
                get {   if(this.root != null) 
                            return this.root.count;
                        return 0;
                    }
            }

            internal RedBlackTreeIterator begin                               //First item in list
            { 
                get {
                        List<RBNode> stack = new List<RBNode>();
                        RBNode n = this.root;
                        while( n != null ) {
                          stack.Add(n);
                          n = n.left;
                        }
                        return new RedBlackTreeIterator(this, stack);
                    }
            }

            internal RedBlackTreeIterator end
            { //Last item in list
                get {
                        List<RBNode> stack = new List<RBNode>();
                        RBNode n = this.root;
                        while( n != null ) {
                          stack.Add(n);
                        n = n.right;
                    }
                    return new RedBlackTreeIterator(this, stack);
                }
            }



            //Insert a new item into the tree
            internal RedBlackTree insert(SweepEvent key, SweepEvent value) 
            {
                Comporator cmp = this.compare;
                //Find point to insert new node at
                RBNode n1 = this.root;
                List<RBNode> n_stack = new List<RBNode>();
                List<int?> d_stack = new List<int?>();
                while(n1 != null)
                {
                    int? d = cmp(key, n1.key);
                    n_stack.Add(n1);
                    d_stack.Add(d);
                    if(d <= 0) 
                        n1 = n1.left;
                    else 
                        n1 = n1.right;
                }

                //Rebuild path to leaf node
                n_stack.Add(new RBNode(RBTreeNodeColor.RED, key, value, null, null, 1));
                for(int i= n_stack.Count-2; i > -1; i--)
                {
                    RBNode n_i = n_stack[i];
                    if(d_stack[i] <= 0)
                        n_stack[i] = new RBNode(n_i.color, n_i.key, n_i.value, n_stack[i+1], n_i.right, n_i.count+1);
                    else 
                        n_stack[i] = new RBNode(n_i.color, n_i.key, n_i.value, n_i.left, n_stack[i+1], n_i.count+1);
                }

                //Rebalance tree using rotations
                for(int i= n_stack.Count-1; i > 1; i--) 
                {
                    RBNode p = n_stack[i-1];
                    RBNode n = n_stack[i];
                    if(p.color == RBTreeNodeColor.BLACK || n.color == RBTreeNodeColor.BLACK) 
                        break;

                    RBNode pp = n_stack[i-2];
                    if(pp.left == p)
                    {
                        if(p.left == n) 
                        {
                            RBNode y = pp.right;
                            if( y != null && y.color == RBTreeNodeColor.RED)
                            {
                                p.color = RBTreeNodeColor.BLACK;
                                pp.right = y.repaint(RBTreeNodeColor.BLACK);
                                pp.color = RBTreeNodeColor.RED;
                                i--;
                            } 
                            else {
                                pp.color = RBTreeNodeColor.RED;
                                pp.left = p.right;
                                p.color = RBTreeNodeColor.BLACK;
                                p.right = pp;
                                n_stack[i-2] = p;
                                n_stack[i-1] = n;
                                pp.recount();
                                p.recount();
                                if(i >= 3)
                                {
                                    RBNode ppp = n_stack[i-3];
                                    if(ppp.left == pp)
                                        ppp.left = p;
                                    else
                                        ppp.right = p;
                                }
                                break;
                            }
                        } 
                        else {
                            RBNode y = pp.right;
                            if(y != null && y.color == RBTreeNodeColor.RED)
                            {
                                p.color = RBTreeNodeColor.BLACK;
                                pp.right = y.repaint(RBTreeNodeColor.BLACK);
                                pp.color = RBTreeNodeColor.RED;
                                i--;
                            } 
                            else {
                                p.right = n.left;
                                pp.color = RBTreeNodeColor.RED;
                                pp.left = n.right;
                                n.color = RBTreeNodeColor.BLACK;
                                n.left = p;
                                n.right = pp;
                                n_stack[i-2] = n;
                                n_stack[i-1] = p;
                                pp.recount();
                                p.recount();
                                n.recount();
                                if(i >= 3)
                                {
                                    RBNode ppp = n_stack[i-3];
                                    if(ppp.left == pp)
                                        ppp.left = n;
                                    else 
                                        ppp.right = n;
                                }
                                break;
                            }
                        }
                    } 
                    else {
                        if(p.right == n) {
                            RBNode y = pp.left;
                            if( y != null && y.color == RBTreeNodeColor.RED) 
                            {
                                p.color = RBTreeNodeColor.BLACK;
                                pp.left = y.repaint(RBTreeNodeColor.BLACK);
                                pp.color = RBTreeNodeColor.RED;
                                i--;
                            } 
                            else {
                                pp.color = RBTreeNodeColor.RED;
                                pp.right = p.left;
                                p.color = RBTreeNodeColor.BLACK;
                                p.left = pp;
                                n_stack[i-2] = p;
                                n_stack[i-1] = n;
                                pp.recount();
                                p.recount();
                                if(i >= 3) 
                                {
                                    RBNode ppp = n_stack[i-3];
                                    if(ppp.right == pp)
                                        ppp.right = p;
                                     else 
                                        ppp.left = p;
                                }
                                break;
                            }
                        } 
                        else {
                            RBNode y = pp.left;
                            if(y != null && y.color == RBTreeNodeColor.RED)
                            {
                                p.color = RBTreeNodeColor.BLACK;
                                pp.left = y.repaint(RBTreeNodeColor.BLACK);
                                pp.color = RBTreeNodeColor.RED;
                                i--;
                            } 
                            else {
                                p.left = n.right;
                                pp.color = RBTreeNodeColor.RED;
                                pp.right = n.left;
                                n.color = RBTreeNodeColor.BLACK;
                                n.right = p;
                                n.left = pp;
                                n_stack[i-2] = n;
                                n_stack[i-1] = p;
                                pp.recount();
                                p.recount();
                                n.recount();
                                if(i >= 3)
                                {
                                    RBNode ppp = n_stack[i-3];
                                    if(ppp.right == pp) 
                                        ppp.right = n;
                                    else 
                                        ppp.left = n;
                                }
                                break;
                            }
                        }
                    }
                }
                //Return new tree
                n_stack[0].color = RBTreeNodeColor.BLACK;
                return (new RedBlackTree(cmp, n_stack[0]));
            }

            //Visit all nodes inorder
            internal List<SweepEvent> doVisitFull(List<SweepEvent> list, Visit visit, RBNode node)
            {
                if(node.left != null) 
                {
                    var v1 = doVisitFull(list, visit, node.left);
                    if(v1 != null ) 
                    { 
                        return v1;
                    }
                }

                var v2 = visit(node.key, node.value, list);
                if(v2 != null )
                { 
                    return v2;
                }
                
                if(node.right != null)
                {
                    return doVisitFull(list, visit, node.right);
                }
                
                return null;
            }

            //Visit half nodes in order
            internal List<SweepEvent> doVisitHalf(SweepEvent lo, Comporator compare, List<SweepEvent> list, Visit visit, RBNode node)
            {
                var l = compare(lo, node.key);
                if(l <= 0) {
                    if(node.left != null) {
                        var v1 = doVisitHalf(lo, compare, list, visit, node.left);
                        if(v1 != null) 
                            return v1;
                    }
                    var v2 = visit(node.key, node.value, list);
                    if(v2 != null)
                        return v2;
                }
                if(node.right != null)
                {
                    return doVisitHalf(lo, compare, list, visit, node.right);
                }
                return null;
            }

            //Visit all nodes within a range
            internal List<SweepEvent> doVisit(SweepEvent lo, SweepEvent hi, Comporator compare, List<SweepEvent> list, Visit visit, RBNode node)
            {
                var l = compare(lo, node.key);
                var h = compare(hi, node.key);
                if(l <= 0) 
                {
                    if(node.left != null)
                    {
                        var v1 = doVisit(lo, hi, compare, list, visit, node.left);
                        if (v1 != null)
                            return v1;
                    }
                    if(h > 0) 
                    {
                        var v2 = visit(node.key, node.value, list);
                        if(v2 != null)
                            return v2;
                    }
                }
                if( h > 0  &&  node.right != null)
                {
                    return doVisit(lo, hi, compare, list, visit, node.right);
                }
                return null;
            }

            // Правильнее было назвать GetList()
            internal void ForEach(List<SweepEvent> list, Visit visit, SweepEvent lo = null, SweepEvent hi = null)
            {
                if(this.root == null) {
                    return;
                }
                int arguments_length= 3;
                if(hi==null) arguments_length--;
                if(lo==null) arguments_length--;
                if(visit== null) return;

                switch(arguments_length) 
                {
                    case 1:
                        doVisitFull(list, visit, this.root);
                        break;

                    case 2:
                        doVisitHalf(lo, this.compare, list, visit, this.root);
                        break;

                    case 3:
                        if(this.compare(lo, hi) >= 0) 
                            return;
                        
                        doVisit(lo, hi, this.compare, list, visit, this.root);
                        break;
                }
            }

            //Find the ith item in the tree
            internal RedBlackTreeIterator at(long idx)
             {
                if(idx < 0) {
                    return (new RedBlackTreeIterator(this, new List<RBNode>()));
                }
              
                var n = this.root;
                var stack = new List<RBNode>();
                while(true) {
                    stack.Add(n);
                    if(n.left != null) 
                    {
                        if(idx < n.left.count) {
                            n = n.left;
                            continue;
                        }
                        idx -= n.left.count;
                    }
                    if( idx < 0 ) {
                        return (new RedBlackTreeIterator(this, stack));
                    }
                    idx--;
                    if(n.right != null)
                    {
                        if(idx >= n.right.count) {
                            break;
                        }
                        n = n.right;
                    }
                    else
                        break;
                }
                return (new RedBlackTreeIterator(this, new List<RBNode>()));
            }

            //Finds the item with key if it exists
             public RedBlackTreeIterator find(SweepEvent key)
            {
                var cmp = this.compare;
                var n = this.root;
                List<RBNode> stack = new List<RBNode>();
                while(n != null) {
                    var d = cmp(key, n.key);
                    stack.Add(n);
                    if(d == 0) 
                        return (new RedBlackTreeIterator(this, stack));
                    else if(d < 0) 
                        n = n.left;
                    else
                        n = n.right;
                }
                return (new RedBlackTreeIterator(this, new List<RBNode>()));
            }

            //Removes item with key from tree
            internal RedBlackTree remove(SweepEvent key)
            {
                var iter = this.find(key);
                if(iter != null) 
                {
                    return iter.remove();
                }
                return this;
            }

            //Returns the item at `key`
            internal SweepEvent get(SweepEvent key)
            {
                var cmp = this.compare;
                var n = this.root;
                while(n != null) {
                    var d = cmp(key, n.key);
                    if(d == 0) 
                        return n.value;
                    else if(d < 0) 
                        n = n.left;
                    else 
                        n = n.right;
                }
                return null;
            }

            /*
            internal RedBlackTreeIterator ge( SweepEvent key)
            {
                var cmp = this.compare;
                var n = this.root;
                var stack = new List<RBNode>();
                var last_ptr = 0;
                while(n != null) {
                    var d = cmp(key, n.key);
                    stack.Add(n);
                    if(d <= 0)
                    {
                        last_ptr = stack.Count;
                    }
                    if(d <= 0)
                        n = n.left;
                    else 
                        n = n.right;
                }
                //stack.length = last_ptr;
                return (new RedBlackTreeIterator(this, stack));
            }

            internal RedBlackTreeIterator gt(SweepEvent key)
            {
                var cmp = this.compare;
                var n = this.root;
                var stack = new List<RBNode>();
                var last_ptr = 0;
                while(n != null) {
                    var d = cmp(key, n.key);
                    stack.Add(n);
                    if(d < 0) 
                    {
                        last_ptr = stack.Count;
                    }
                    
                    if(d < 0) 
                        n = n.left;
                    else
                        n = n.right;
                }
                //stack.length = last_ptr;
                return new RedBlackTreeIterator(this, stack);
            }

            internal RedBlackTreeIterator lt(SweepEvent key)
            {
                var cmp = this.compare;
                var n = this.root;
                var stack = new List<RBNode>();
                var last_ptr = 0;
                while(n != null) {
                    var d = cmp(key, n.key);
                    stack.Add(n);
                    if(d > 0) 
                    {
                        last_ptr = stack.Count;
                    }
                
                    if(d <= 0) 
                        n = n.left;
                    else
                        n = n.right;
                }
                //stack.length = last_ptr;
                return (new RedBlackTreeIterator(this, stack));
            }

            internal RedBlackTreeIterator le(SweepEvent key)
            {
                var cmp = this.compare;
                var n = this.root;
                var stack = new List<RBNode>();
                var last_ptr = 0;
                while(n != null) {
                    var d = cmp(key, n.key);
                    stack.Add(n);
                    if(d >= 0) 
                    {
                        last_ptr = stack.Count;
                    }
                
                    if(d < 0)
                        n = n.left;
                    else 
                        n = n.right;
                }
                //stack.Count = last_ptr;
                return new RedBlackTreeIterator(this, stack);
            }*/
        }


        //Iterator for red black tree
        internal class RedBlackTreeIterator
        {
            internal RedBlackTree tree;
            internal List<RBNode> _stack;

            internal RedBlackTreeIterator(RedBlackTree tree, List<RBNode> stack)
            {
                this.tree = tree;
                this._stack = stack;
            }

//var iproto = RedBlackTreeIterator.prototype

            internal bool valid { get { return (this._stack.Count > 0); }}             // Test if iterator is valid

            internal RBNode node
            {                                                   // Node of the iterator
                get {   if(this._stack.Count > 0) 
                            return this._stack[this._stack.Count-1];
                        return null;
                    }
              //, enumerable: true ??????
            }

            internal SweepEvent key
            {                                                          // Returns key
                get{
                    if (this._stack.Count > 0)
                        return this._stack[this._stack.Count - 1].key;
                    return null;
                }
              //, enumerable: true
            }

            internal SweepEvent value
            {                                                      //Returns value
                get {   if(this._stack.Count > 0) 
                            return this._stack[this._stack.Count-1].value;
                        return null;
                }
                //, enumerable: true
            }

            internal long index {                                                       //Returns the position of this iterator in the sorted list
                get {   long idx = 0;
                        var stack = this._stack;
                        if( stack.Count == 0) {
                            var r = this.tree.root;
                            if( r != null) {
                                return r.count;
                            }
                            return 0;
                        } 
                        else if(stack[stack.Count-1].left != null)
                        {
                            idx = stack[stack.Count-1].left.count;
                        }
                        for(int i=stack.Count-2; i > -1; i--)
                        {
                            if(stack[i+1] == stack[i].right) {
                                idx++;
                                if(stack[i].left != null) 
                                    idx += stack[i].left.count;
                            }
                        }
                        return idx;
              }
              //, enumerable: true
            }

            internal bool hasNext{                                                      //Checks if iterator is at end of tree
                get {   var stack = this._stack;
                        if( stack.Count == 0) {
                            return false;
                        }
                        if( stack[stack.Count-1].right != null)
                        {
                            return true;
                        }
                        
                    for(int i=stack.Count-1; i > 0; i--)
                        {
                            if(stack[i-1].left == stack[i])
                                return true;
                        }
                        return false;
                }
            }

            internal bool hasPrev{                                                      //Checks if iterator is at start of tree
                get {   var stack = this._stack;
                        if(stack.Count == 0) {
                            return false;
                        }
                        if(stack[stack.Count-1].left != null) {
                            return true;
                        }
                        for(int i=stack.Count-1; i > 0; i--)
                        {
                            if(stack[i-1].right == stack[i])
                                return true;
                        }
                    return false;
                }
            }


            //Advances iterator to next element in list
            internal void next()
            {
                var stack = this._stack;
                if(stack.Count == 0) {
                    return;
                }
                var n = stack[stack.Count-1];
                if(n.right != null) {
                    n = n.right;
                    while(n != null) {
                        stack.Add(n);
                        n = n.left;
                    }
                }
                else {
                    stack.RemoveAt(stack.Count-1);
                    while( stack.Count > 0  &&  stack[stack.Count-1].right == n) {
                        n = stack[stack.Count-1];
                        stack.RemoveAt(stack.Count-1);
                    }
                }
            }

            //Moves iterator backward one element
            internal void prev()
            {
                var stack = this._stack;
                if(stack.Count == 0) {
                    return;
                }
                var n = stack[stack.Count-1];
                if(n.left != null) {
                    n = n.left;
                    while(n != null) {
                        stack.Add(n);
                        n = n.right;
                    }
                } 
                else {
                    stack.RemoveAt(stack.Count-1);
                    while( stack.Count > 0  &&  stack[stack.Count-1].left == n)
                    {
                        n = stack[stack.Count-1];
                        stack.RemoveAt(stack.Count-1);
                    }
                }
            }

            ////Makes a copy of an iterator
            //internal RedBlackTreeIterator clone()
            //{
                
            //    return (new RedBlackTreeIterator(this.tree, this._stack.slice()));
            //}

            ///Removes item at iterator from tree
            internal RedBlackTree remove()
            {
                var stack = this._stack;
                if(stack.Count == 0) {
                    return this.tree;
                }
                //First copy path to node
                var cstack = new List<RBNode>(stack);//new List<RBNode>(stack.Count);
                var n = stack[stack.Count-1];
                cstack[cstack.Count-1] = new RBNode(n.color, n.key, n.value, n.left, n.right, n.count);
                for(int i= stack.Count-2; i > -1; i--) 
                {
                    var n_i = stack[i];
                    if(n_i.left == stack[i+1]) 
                        cstack[i] = new RBNode(n_i.color, n_i.key, n_i.value, cstack[i+1], n_i.right, n_i.count);
                    else 
                        cstack[i] = new RBNode(n_i.color, n_i.key, n_i.value, n_i.left, cstack[i+1], n_i.count);
                }

                //Get node
                n = cstack[cstack.Count-1];

                //If not leaf, then swap with previous node
                if(n.left != null  && n.right != null) {
                    //First walk to previous leaf
                    var split = cstack.Count;
                    n = n.left;
                    while(n.right != null) {
                        cstack.Add(n);
                        n = n.right;
                    }
                    //Copy path to leaf
                    var v = cstack[split-1];
                    cstack.Add(new RBNode(n.color, v.key, v.value, n.left, n.right, n.count));
                    cstack[split-1].key = n.key;
                    cstack[split-1].value = n.value;

                    //Fix up stack
                    for(int i= cstack.Count-2; i >= split; i--)
                    {
                        n = cstack[i];
                        cstack[i] = new RBNode(n.color, n.key, n.value, n.left, cstack[i+1], n.count);
                    }
                    cstack[split-1].left = cstack[split];
                }

                //Remove leaf node
                n = cstack[cstack.Count-1];
                if(n.color == RBTreeNodeColor.RED)
                {
                    //Easy case: removing red leaf
                    var p = cstack[cstack.Count-2];
                    if(p.left == n) 
                        p.left = null;
                    else if(p.right == n) 
                        p.right = null;
                
                    cstack.RemoveAt(cstack.Count-1);
                    for(int i= 0; i < cstack.Count; i++) {
                        cstack[i].count--;
                    }
                    return (new RedBlackTree(this.tree.compare, cstack[0]));
                } 
                else
                {
                    if(n.left != null || n.right != null) {
                        //Second easy case:  Single child black parent
                        if(n.left != null) 
                            swapNode(n, n.left);
                        else if(n.right != null )
                            swapNode(n, n.right);
                  
                        //Child must be red, so repaint it black to balance color
                        n.color = RBTreeNodeColor.BLACK;
                        for(int i= 0; i < cstack.Count-1; i++)
                        {
                            cstack[i].count--;
                        }
                        return (new RedBlackTree(this.tree.compare, cstack[0]));
                    } 
                    else if(cstack.Count == 1){
                        //Third easy case: root
                        return new RedBlackTree(this.tree.compare, null);
                    } 
                    else {
                        //Hard case: Repaint n, and then do some nasty stuff
                        for(int i= 0; i < cstack.Count; i++) 
                        {
                            cstack[i].count--;
                        }
                        var parent = cstack[cstack.Count-2];
                        fixDoubleBlack(cstack);
                        //Fix up links
                        if(parent.left == n) 
                            parent.left = null;
                        else 
                            parent.right = null;
                    }
                }
                return new RedBlackTree(this.tree.compare, cstack[0]);
            }

            //Update value
            internal RedBlackTree update(SweepEvent value)
            {
                var stack = this._stack;
                if(stack.Count == 0) {
                    throw new Exception("Can't update empty node!");
                }
                var cstack = new List<RBNode>(stack.Count);
                var n = stack[stack.Count-1];
                cstack[cstack.Count-1] = new RBNode(n.color, n.key, value, n.left, n.right, n.count);
                for(int i= stack.Count-2; i > -1; i--) 
                {
                    n = stack[i];
                    if(n.left == stack[i+1]) 
                        cstack[i] = new RBNode(n.color, n.key, n.value, cstack[i+1], n.right, n.count);
                    else 
                        cstack[i] = new RBNode(n.color, n.key, n.value, n.left, cstack[i+1], n.count);
                }
                return new RedBlackTree(this.tree.compare, cstack[0]);
            }

            //Swaps two nodes
            internal void swapNode(RBNode n, RBNode v)
            {
              n.key = v.key;
              n.value = v.value;
              n.left = v.left;
              n.right = v.right;
              n.color = v.color;
              n.count = v.count;
            }

            //Fix up a double black node in a tree
            internal void fixDoubleBlack( List<RBNode>stack )
            {
                RBNode n, p, s, z;
                
                for(int i= stack.Count-1; i > -1; i--)
                {
                    n = stack[i];
                    if(i == 0) {
                        n.color = RBTreeNodeColor.BLACK;
                        return;
                    }
                    
                    p = stack[i-1];
                    if(p.left == n)
                    {
                        s = p.right;
                        if( s.right != null  &&  s.right.color == RBTreeNodeColor.RED) 
                        {
                            s = p.right = s.cloneNode();
                            z = s.right = s.right.cloneNode();
                            p.right = s.left;
                            s.left = p;
                            s.right = z;
                            s.color = p.color;
                            n.color = RBTreeNodeColor.BLACK;
                            p.color = RBTreeNodeColor.BLACK;
                            z.color = RBTreeNodeColor.BLACK;
                            p.recount();
                            s.recount();
                            if(i > 1) 
                            {
                                var pp = stack[i-2];
                                if(pp.left == p)
                                    pp.left = s;
                                else
                                    pp.right = s;
                            }
                            stack[i-1] = s;
                            return;
                        }
                        else if(s.left != null  &&  s.left.color == RBTreeNodeColor.RED) 
                        {
                            s = p.right = s.cloneNode();
                            z = s.left = s.left.cloneNode();
                            p.right = z.left;
                            s.left = z.right;
                            z.left = p;
                            z.right = s;
                            z.color = p.color;
                            p.color = RBTreeNodeColor.BLACK;
                            s.color = RBTreeNodeColor.BLACK;
                            n.color = RBTreeNodeColor.BLACK;
                            p.recount();
                            s.recount();
                            z.recount();
                            if(i > 1) 
                            {
                                var pp = stack[i-2];
                                if(pp.left == p) 
                                    pp.left = z;
                                else
                                    pp.right = z;
                            }
                            stack[i-1] = z;
                            return;
                        }
                        
                        if(s.color == RBTreeNodeColor.BLACK) 
                        {
                            if(p.color == RBTreeNodeColor.RED) 
                            {
                                p.color = RBTreeNodeColor.BLACK;
                                p.right = s.repaint(RBTreeNodeColor.RED);
                                return;
                            }
                            else{
                                p.right = s.repaint(RBTreeNodeColor.RED);
                                continue;
                            }
                        } 
                        else{
                            s = s.cloneNode();
                            p.right = s.left;
                            s.left = p;
                            s.color = p.color;
                            p.color = RBTreeNodeColor.RED;
                            p.recount();
                            s.recount();
                            if(i > 1)
                            {
                                var pp = stack[i-2];
                                if(pp.left == p)
                                    pp.left = s;
                                else 
                                    pp.right = s;
                            }
                            stack[i-1] = s;
                            stack[i] = p;
                            if(i+1 < stack.Count)
                                stack[i+1] = n;
                            else
                                stack.Add(n);
                            i = i+2;
                        }
                    }
                    else {
                        s = p.left;
                        if(s.left != null  &&  s.left.color == RBTreeNodeColor.RED)
                        {
                            s = p.left = s.cloneNode();
                            z = s.left = s.left.cloneNode();
                            p.left = s.right;
                            s.right = p;
                            s.left = z;
                            s.color = p.color;
                            n.color = RBTreeNodeColor.BLACK;
                            p.color = RBTreeNodeColor.BLACK;
                            z.color = RBTreeNodeColor.BLACK;
                            p.recount();
                            s.recount();
                            if(i > 1) 
                            {
                                var pp = stack[i-2];
                                if(pp.right == p) 
                                    pp.right = s;
                                else
                                    pp.left = s;
                            }
                            stack[i-1] = s;
                            return;
                        }
                        else if( s.right != null  &&  s.right.color == RBTreeNodeColor.RED)
                        {
                            s = p.left = s.cloneNode();
                            z = s.right = s.right.cloneNode();
                            p.left = z.right;
                            s.right = z.left;
                            z.right = p;
                            z.left = s;
                            z.color = p.color;
                            p.color = RBTreeNodeColor.BLACK;
                            s.color = RBTreeNodeColor.BLACK;
                            n.color = RBTreeNodeColor.BLACK;
                            p.recount();
                            s.recount();
                            z.recount();
                            if(i > 1) 
                            {
                                var pp = stack[i-2];
                                if(pp.right == p)
                                    pp.right = z;
                                else
                                    pp.left = z;
                            }
                            stack[i-1] = z;
                            return;
                        }
                        
                        if(s.color == RBTreeNodeColor.BLACK)
                        {
                            if(p.color == RBTreeNodeColor.RED)
                            {
                                p.color = RBTreeNodeColor.BLACK;
                                p.left = s.repaint(RBTreeNodeColor.RED);
                                return;
                            } 
                            else {
                                p.left = s.repaint(RBTreeNodeColor.RED);
                                continue;
                            }
                        }
                        else {
                            s = s.cloneNode();
                            p.left = s.right;
                            s.right = p;
                            s.color = p.color;
                            p.color = RBTreeNodeColor.RED;
                            p.recount();
                            s.recount();
                            if(i > 1) 
                            {
                                var pp = stack[i-2];
                                if(pp.right == p)
                                    pp.right = s;
                                else
                                    pp.left = s;
                            }
                            stack[i-1] = s;
                            stack[i] = p;
                            if(i+1 < stack.Count) 
                                stack[i+1] = n;
                            else 
                                stack.Add(n);
                            i = i+2;
                        }
                    }
                }
            }
        }
    }
}