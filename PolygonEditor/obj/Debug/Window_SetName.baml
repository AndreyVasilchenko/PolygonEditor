   M S B A M L   `   `   `  ����  PolygonEditor"    PolygonEditor.Window_SetName   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Icon$  images/Simple.ico��5   	   $	��128��5   	   $	��333��6    � WindowStartupLocation$ CenterOwner=�6"    � 
ResizeMode$ NoResize=�6F    � Topmost .  � 	Resources 5       �� 
TargetType �� Property �� Value��%�   '��       �� 5   
   " ��6   .�5	      ��  � ��$ 60��6)   �� 5
       � ��$ 20��6*   �� 5       � ��$ 5,0,10,5q�6'   5      5      .��5      � $��0,0,0,1q�6   ��5   
   �� 5       �� ImageSource$ images\bg_46.jpg��6   	 j� Viewport$	 0,0,256,256�68   
 j� ViewportUnits$
 Absolute=�6O    j� TileMode$
 Tile=�6h    � RowDefinitions 5   
   �� 5      $��6* �6   �� 5      $��5* �6    � ColumnDefinitions 5   
   �� 5      $��55 �6   �� 5      .+�5   
   �� -   5        $��
Label_Name��5      $��Name:��6"   $
��5,12q�62   �� 5   
   $��1��60   $��
5,12,10,12q�6@   ��6   �� *InstanceName+6@   �� 5!   
    �� Orientation$ 
Horizontal=�6   $��1��6/   $
��Left=�6<   $��35,5,0,5q�6W   $	��160��6i   $��1��6u   .+�5"      �� -   5         �� 	IsDefault . .��5"   ;   OK6?   �� 5#       �� IsCancel . .��6%   Cancel6-   5$      5%      5&      