   M S B A M L   `   `   `  ����   PolygonEditor'    !PolygonEditor.Window_LoadInstance   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $��WinLoadInsance��  � Title$!  Load Instance from DataBase�� � Icon$ images/Simple.ico��$	��467��$	��369�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=� � Topmost .  � 	Resources  �� 
TargetType �� Property �� Value��%X   '��       �� " ��.���  � ��$ 60����  � ��$ 30����  � ��$ 5q�.��� ���� 	 �� ImageSource$	 images\bg_46.jpg��
 j� Viewport$
 0,0,256,256� j� ViewportUnits$ Absolute=� j� TileMode$
 Tile=� � RowDefinitions �� $
��385* ��� $	��54* �.+��� -   $��CollectionSelector��$��
20,14,19,0q�.@��� $��Shapes��.��&   System.Windows.Controls.DataGrid  -   $��
ShapesGrid��$��0,0,0,0q�   AutoGenerateColumns .     
IsReadOnly .    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Id��   
Visibility$ Hidden=�1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Id+  $
 Name��   MinWidth$	 296Q�   MaxWidth$	 296Q� �� *Name+�� $��ShapeGroups��.��  -   $��ShapeGroupsGrid��$��0,0,0,0q� .   .    $ Id��$ Hidden=� �� *Id+  $
 Name��$	 296Q�$	 296Q� �� *Name+�� $��PolygonOperations��.��  -   $��PolygonOperationsGrid��$��0,0,0,0q� .   .    $ Id��$ Hidden=� �� *Id+  $
 Name��$	 296Q�$	 296Q� �� *Name+��  �� Orientation$ 
Horizontal=�$��1��$��Center=�$��40��$��	62,5,10,9q�$	��280��.+��� -   $��btnLoad��$
��Load�� �� 	IsDefault . �� -   $��	btnCancel��$��Cancel�� �� IsCancel . �� -   $��	btnRename��$��Rename���� -	   $��	btnDelete��$��Delete��