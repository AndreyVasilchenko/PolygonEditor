   M S B A M L   `   `   `  ����  PolygonEditor'    !PolygonEditor.Window_LoadInstance   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��WinLoadInsance��5   	     � Title$!  Load Instance from DataBase��5   	    � Icon$ images/Simple.ico��6-   $	��467��5   	   $	��369��6    � WindowStartupLocation$ CenterOwner=�6"    � 
ResizeMode$ NoResize=�6F    � Topmost .  � 	Resources 5       �� 
TargetType �� Property �� Value��%�   '��       �� 5	   
   " ��6   .�5
      ��  � ��$ 60��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   5      5      .��5      � ��5   
   �� 5      	 �� ImageSource$	 images\bg_46.jpg��6   
 j� Viewport$
 0,0,256,256�68    j� ViewportUnits$ Absolute=�6O    j� TileMode$
 Tile=�6h    � RowDefinitions 5   
   �� 5      $
��385* �6   �� 5      $	��54* �6   .+�5   
   �� -   5        $��CollectionSelector��5      $��
20,14,19,0q�6/   .@�5      �� $��Shapes��6   .��5      &   System.Windows.Controls.DataGrid  -   5        $��
ShapesGrid��5      $��0,0,0,0q�6-      AutoGenerateColumns .     
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6-      
Visibility$ Hidden=�6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *Id+6P     5      $
 Name��6-      MinWidth$	 296Q�6;      MaxWidth$	 296Q�6J    6Y   �� *Name+5      5       �� 5!      $��ShapeGroups��6   .��5"        -   5        $��ShapeGroupsGrid��5"      $��0,0,0,0q�62    .   .  5#        5$      $ Id��6-   $ Hidden=�6P    69   �� *Id+6P     5%      $
 Name��6-   $	 296Q�6;   $	 296Q�6J    6Y   �� *Name+5'      5(      �� 5)      $��PolygonOperations��6   .��5*        -   5        $��PolygonOperationsGrid��5*      $��0,0,0,0q�68    .   .  5+        5,      $ Id��6-   $ Hidden=�6P    69   �� *Id+6P     5-      $
 Name��6-   $	 296Q�6;   $	 296Q�6J    6Y   �� *Name+5/      50      51      �� 52   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Center=�6<   $��40��6W   $��	62,5,10,9q�6c   $	��280��6v   .+�53      �� -   5        $��btnLoad��53      $
��Load��6$    �� 	IsDefault . 6D   �� 54      -   5        $��	btnCancel��54      $��Cancel��6&    �� IsCancel . 67   �� 55      -   5        $��	btnRename��55      $��Rename��6&   67   �� 56      -	   5        $��	btnDelete��56      $��Delete��6&   67   57      59      5:      