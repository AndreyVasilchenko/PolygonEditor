   M S B A M L   `   `   `  ����H  DPolygonEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PolygonEditorPolygonEditor  5          PolygonEditor.App   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      X TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      #cclr-namespace:PolygonEditor  5        �� 	Resources  5         draw_point_on  draw_point_off  draw_line_on  draw_line_off  draw_polyLine_on  draw_polyLine_off  draw_polygon_on  draw_polygon_off  mode_select_on 	 mode_select_off 
 mode_new_on  mode_new_off  mode_edit_on  mode_edit_off  mode_polyOper_on  mode_polyOper_off  Save_on  Save_off  Save_All_on  Save_All_off  Print_on  	Print_off  Load_on  Load_off 
 Clr_on  Clr_off 
 Del_on  Del_off'   !PolygonEditor.BooleanConverter_OR  BooleanConverterOR(   "PolygonEditor.BooleanConverter_AND  BooleanConverterAND(   "PolygonEditor.BooleanConverter_NOT  BooleanConverterNOT-   'PolygonEditor.NumericToBooleanConverter  NumericToBooleanConverter0   *PolygonEditor.BooleanToVisibilityConverter    BooleanToVisibilityConverter�   �PolygonEditor.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False '! #BooleanToVisibilityConverterInverse3   -PolygonEditor.BooleanToVisibilityConverter_OR "" BooleanToVisibilityConverterOR�   �PolygonEditor.BooleanConverter_OR`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False/	   )PolygonEditor.ShapeTypeToBooleanConverter # ShapeTypeToBooleanConverter*
   $PolygonEditor.ModeToBooleanConverter $ ModeToBooleanConverter*   $PolygonEditor.ModeToContentConverter % ModeToContentConverter & Button_Save �� 
TargetType �� Property �� Value �� Triggers	 P� Property
 P� Value ' Button_Save_All ( Button_Print ) Button_Load * RButton_modeSelect + RButton_modeNew , RButton_modeEdit !- RButton_modePolygonOperations . 
Button_Del / 
Button_Clr 0 Tool_textBlock 1 Botton_ToggleBotton 2 Botton_Botton 3 Botton_CheckBox 4 Label_Prompt 5 Tool_weightPicker 6 Tool_сolorPicker#  Xceed.Wpf.Toolkit.ColorPicker 7 RButton_drawPoint 8 RButton_drawLine 9 RButton_drawPolyline : RButton_drawPolygon ; StackPanel_Coords < Canvas_Coords = Button_InsertUpdate��%  &	        &	 2     &	 e     &	 �     &	 �     &	 �     &	 3    &	 g    &	 �    &		 �    &	
     &	 3    &	 d    &	 �    &	 �    &	 �    &	 $    &	 P    &	 }    &	 �    &	 �    &	     &	 9    &	 e    &	 �    &	 �    &	 �    &	     &	 @    &	 S    &	 f    &	 y    &	  �    &	! �    &	" �    &	# .    &	$ A    &	% T    &	& g    7  7  &	' W    7  7  &	( G    7  7  &	) 7	    7  7  &	* '
    7 	 7  &	+ *    7  7 
 &	, �    7  7  &	- �    7  7  &	. �    7  7  &	/ �    7  7  &	0 �    &	1 A    &	2     &	3 �    &	4 }    &	5 1    &	6 �    &	7 9    7  7   &	8 <    7  7  &	9 ?    7  7  &	: B    7  7  &	; E    &	< �    &	= M    �� 5   
   $��images/draw_point_on.png��6&   �� 5   
   $��images/draw_point_off.png��6'   �� 5   
   $��images/draw_line_on.png��6%   �� 5   
   $��images/draw_line_off.png��6&   �� 5   
   $!��images/draw_polyLine_on.png��6)   �� 5   
   $"��images/draw_polyLine_off.png��6*   �� 5   
   $ ��images/draw_polygon_on.png��6(   �� 5   
   $!��images/draw_polygon_off.png��6)   �� 5   
   $��images/mode_select_on.png��6'   �� 5   
   $ ��images/mode_select_off.png��6(   �� 5   
   $��images/mode_new_on.png��6$   �� 5   
   $��images/mode_new_off.png��6%   �� 5   
   $��images/mode_edit_on.png��6%   �� 5   
   $��images/mode_edit_off.png��6&   �� 5   
   $��images/split_on1.png��6)   �� 5   
   $��images/split_off1.png��6*   �� 5   
   $��images/save_on.png��6    �� 5   
   $��images/save_off.png��6!   �� 5   
   $��images/save_All_on.png��6$   �� 5   
   $��images/save_All_off.png��6%   �� 5   
   $��images/print_on.png��6!   �� 5    
   $��images/print_off.png��6"   �� 5!   
   $��images/load_on.png��6    �� 5"   
   $��images/load_off.png��6!   �� 5#   
   $��images/Clr_on.png��6   �� 5$   
   $��images/Clr_off.png��6    �� 5%   
   $��images/Del_on.png��6   �� 5&   
   $��images/Del_off.png��6      5)   
   6      5*   
   6!     5+   
   6!     5,   
   6&     5-   
   $ Visible=�6N   $ 	Collapsed=�6]     5.   
   $ Visible=�6U   $ 	Collapsed=�6e     5/   
   $ Visible=�6S   $ 	Collapsed=�6b   	  50   
   6(   
  51   
   6#     52   
   6#   �� 55   
   " ��6%   .�56      ��  � ��8   6(    57      P� 58      	 � ��
 . . �59      ��  � ��8  60   5:      P� 5;      	 � {�
 .  . �5<      ��  �@J�Opacity$	 0.4Q�60   5=      5?      �� 5@   
   " ��6)   .�5A      ��  � ��8   6(    5B      P� 5C      	 � ��
 . . �5D      ��  � ��8  60   5E      P� 5F      	 � {�
 .  . �5G      ��  �@J�Opacity$	 0.4Q�60   5H      5J      �� 5K   
   " ��6&   .�5L      ��  � ��8   6(    5M      P� 5N      	 � ��
 . . �5O      ��  � ��8  60   5P      P� 5Q      	 � {�
 .  . �5R      ��  �@J�Opacity$	 0.4Q�60   5S      5U      �� 5V   
   " ��6%   .�5W      ��  � ��8   6(    5X      P� 5Y      	 � ��
 . . �5Z      ��  � ��8  60   5[      P� 5\      	 � {�
 .  . �5]      ��  �@J�Opacity$	 0.4Q�60   5^      5`      �� 5a   
   " 	�6,   .�5b      ��  � ��8   6(    5c      P� 5d      	 �@d�	IsChecked$

 TrueY�6/   . �5e      ��  � ��8  60   5f      P� 5g      	 � {�
 .  . �5h      ��  �@J�Opacity$	 0.4Q�60   5i      5k      �� 5l   
   " 	�6)   .�5m      ��  � ��8   6(    5n      P� 5o      	 �@d�	IsChecked$

 TrueY�6/   . �5p      ��  � ��8  60   5q      5s      �� 5t   
   " 	�6*   .�5u      ��  � ��8   6(    5v      P� 5w      	 �@d�	IsChecked$

 TrueY�6/   . �5x      ��  � ��8  60   5y      P� 5z      	 � {�
 .  . �5{      ��  �@J�Opacity$	 0.4Q�60   5|      5~      �� 5   
   " 	�67   .�5�      ��  � ��8   6(    5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6/   . �5�      ��  � ��8  60   5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   " ��6$   .�5�      ��  � ��8   6(    5�      P� 5�      	 � ��
 . . �5�      ��  � ��8  60   5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   " ��6$   .�5�      ��  � ��8   6(    5�      P� 5�      	 � ��
 . . �5�      ��  � ��8  60   5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   " ��6'    5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   " d�6-   .�5�      ��  � �� �	#00FFFFFF6+    5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6/   . �5�      ��  � �� �	#FFF3D56F63   5�      5�      �� 5�   
   " ��6'   .�5�      ��  � �� �	#00FFFFFF6+    5�      P� 5�      	 � ��
 . . �5�      ��  � �� �	#FFF3D56F63   5�      5�      �� 5�   
   " ��6)   .�5�      ��  � �� �	#00FFFFFF6+    5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6/   . �5�      ��  � �� �	#FFF3D56F63   5�      5�      �� 5�   
   " ��6%   .�5�      ��  �@J�Opacity$	 0.0Q�6(    5�      P� 5�      	 � {�
 . . �5�      ��  �@J�Opacity$	 0.5Q�60   5�      5�      �� 5�   
   " ��6*    5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   "  6)    5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   " 	�6+   .�5�      ��  � ��8   6(    5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6/   . �5�      ��  � ��8  60   5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   " 	�6*   .�5�      ��  � ��8   6(    5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6/   . �5�      ��  � ��8  60   5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5�   
   " 	�6.   .�5�      ��  � ��8   6(    5�      P� 5�      	 �@d�	IsChecked$

 TrueY�6/   . �5�      ��  � ��8  60   5�      P� 5�      	 � {�
 .  . �5�      ��  �@J�Opacity$	 0.4Q�60   5�      5�      �� 5   
   " 	�6-   .�5     ��  � ��8   6(    5     P� 5     	 �@d�	IsChecked$

 TrueY�6/   . �5     ��  � ��8  60   5     P� 5     	 � {�
 .  . �5     ��  �@J�Opacity$	 0.4Q�60   5     5
     �� 5  
   " ��6*    5     P� 5     	 � {�
 .  . �5     ��  �@J�Opacity$	 0.4Q�60   5     5     �� 5  
   " ��6&    5     P� 5     	 � {�
 .  . �5     ��  �@J�Opacity$	 0.4Q�60   5     5     �� 5  
   " ��6-   .�5     ��  �@�ToolTip$  ��6(   �� 5      � y�$ 	Collapsed=�6+    5      P� 5!     	 � ��$
 InsertShape��6-   . �5"     ��  �@�ToolTip$ Insert a new shape��60   �� 5#      � y�$ Visible=�63   5$     P� 5%     	 � ��$
 UpdateCoords��6-   . �5&     ��  �@�ToolTip$2 ,Update the coordinates of the selected shape��60   �� 5'      � y�$ Visible=�63   5(     5*     57     59     