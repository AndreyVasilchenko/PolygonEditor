   M S B A M L   `   `   `  ���� H  DPolygonEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PolygonEditorPolygonEditor      PolygonEditor.App   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     X TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  #cclr-namespace:PolygonEditor    �� 	Resources     draw_point_on  draw_point_off  draw_line_on  draw_line_off  draw_polyLine_on  draw_polyLine_off  draw_polygon_on  draw_polygon_off  mode_select_on 	 mode_select_off 
 mode_new_on  mode_new_off  mode_edit_on  mode_edit_off  mode_polyOper_on  mode_polyOper_off  Save_on  Save_off  Save_All_on  Save_All_off  Print_on  	Print_off  Load_on  Load_off 
 Clr_on  Clr_off 
 Del_on  Del_off'   !PolygonEditor.BooleanConverter_OR  BooleanConverterOR(   "PolygonEditor.BooleanConverter_AND  BooleanConverterAND(   "PolygonEditor.BooleanConverter_NOT  BooleanConverterNOT-   'PolygonEditor.NumericToBooleanConverter  NumericToBooleanConverter0   *PolygonEditor.BooleanToVisibilityConverter    BooleanToVisibilityConverter�   �PolygonEditor.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False '! #BooleanToVisibilityConverterInverse3   -PolygonEditor.BooleanToVisibilityConverter_OR "" BooleanToVisibilityConverterOR�   �PolygonEditor.BooleanConverter_OR`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False/	   )PolygonEditor.ShapeTypeToBooleanConverter # ShapeTypeToBooleanConverter*
   $PolygonEditor.ModeToBooleanConverter $ ModeToBooleanConverter*   $PolygonEditor.ModeToContentConverter % ModeToContentConverter & Button_Save �� 
TargetType �� Property �� Value �� Triggers	 P� Property
 P� Value ' Button_Save_All ( Button_Print ) Button_Load * RButton_modeSelect + RButton_modeNew , RButton_modeEdit !- RButton_modePolygonOperations . 
Button_Del / 
Button_Clr 0 Tool_textBlock 1 Botton_ToggleBotton 2 Botton_Botton 3 Botton_CheckBox 4 Label_Prompt 5 Tool_weightPicker 6 Tool_сolorPicker#  Xceed.Wpf.Toolkit.ColorPicker 7 RButton_drawPoint 8 RButton_drawLine 9 RButton_drawPolyline : RButton_drawPolygon ; StackPanel_Coords < Canvas_Coords = Button_InsertUpdate��%�  &	        &	 $     &	 I     &	 l     &	 �     &	 �     &	 �     &	     &	 ,    &		 Q    &	
 w    &	 �    &	 �    &	 �    &	     &	 #    &	 D    &	 b    &	 �    &	 �    &	 �    &	 �    &	     &	 #    &	 B    &	 _    &	 }    &	 �    &	 �    &	 �    &	 �    &	 �    &	  �    &	! �    &	"     &	# 5    &	$ :    &	% ?    &	& D    7  7  &	' �    7  7  &	( H    7  7  &	) �    7  7  &	* L    7 	 7  &	+ �    7  7 
 &	, 6    7  7  &	- �    7  7  &	. V    7  7  &	/ �    7  7  &	0 Z	    &	1 �	    &	2 
    &	3 t
    &	4 �
    &	5 L    &	6 �    &	7 �    7  7   &	8 d    7  7  &	9 �    7  7  &	: �    7  7  &	;     &	< X    &	= �    �� $��images/draw_point_on.png���� $��images/draw_point_off.png���� $��images/draw_line_on.png���� $��images/draw_line_off.png���� $!��images/draw_polyLine_on.png���� $"��images/draw_polyLine_off.png���� $ ��images/draw_polygon_on.png���� $!��images/draw_polygon_off.png���� $��images/mode_select_on.png���� $ ��images/mode_select_off.png���� $��images/mode_new_on.png���� $��images/mode_new_off.png���� $��images/mode_edit_on.png���� $��images/mode_edit_off.png���� $��images/split_on1.png���� $��images/split_off1.png���� $��images/save_on.png���� $��images/save_off.png���� $��images/save_All_on.png���� $��images/save_All_off.png���� $��images/print_on.png���� $��images/print_off.png���� $��images/load_on.png���� $��images/load_off.png���� $��images/Clr_on.png���� $��images/Clr_off.png���� $��images/Del_on.png���� $��images/Del_off.png��          $ Visible=�$ 	Collapsed=�  $ Visible=�$ 	Collapsed=�  $ Visible=�$ 	Collapsed=�	  
    �� " ��.���  � ��8    P� 	 � ��
 . . ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " ��.���  � ��8    P� 	 � ��
 . . ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " ��.���  � ��8    P� 	 � ��
 . . ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " ��.���  � ��8    P� 	 � ��
 . . ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  �� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " ��.���  � ��8    P� 	 � ��
 . . ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " ��.���  � ��8    P� 	 � ��
 . . ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " �� P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " d�.���  � �� �	#00FFFFFF P� 	 �@d�	IsChecked$

 TrueY�. ���  � �� �	#FFF3D56F�� " ��.���  � �� �	#00FFFFFF P� 	 � ��
 . . ���  � �� �	#FFF3D56F�� " ��.���  � �� �	#00FFFFFF P� 	 �@d�	IsChecked$

 TrueY�. ���  � �� �	#FFF3D56F�� " ��.���  �@J�Opacity$	 0.0Q� P� 	 � {�
 . . ���  �@J�Opacity$	 0.5Q��� " �� P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� "   P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " 	�.���  � ��8    P� 	 �@d�	IsChecked$

 TrueY�. ���  � ��8  P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " �� P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " �� P� 	 � {�
 .  . ���  �@J�Opacity$	 0.4Q��� " ��.���  �@�ToolTip$  ����  � y�$ 	Collapsed=� P� 	 � ��$
 InsertShape��. ���  �@�ToolTip$ Insert a new shape����  � y�$ Visible=�P� 	 � ��$
 UpdateCoords��. ���  �@�ToolTip$2 ,Update the coordinates of the selected shape����  � y�$ Visible=�