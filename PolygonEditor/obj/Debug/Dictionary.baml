   M S B A M L   `   `   `  ����H  DPolygonEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PolygonEditorPolygonEditor  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   GlassButton  �� 
TargetType � 	Resources  Key �� 
TargetName �� TargetProperty g� 	BeginTime O� KeyTime O� Value ��Name �� CornerRadius	 � RowDefinitions
 J� Opacity �� RelativeTransform �� ScaleX �� ScaleY �� AngleX �� AngleY �� Angle T� X T� Y � Color � Offset �� 
StartPoint �� EndPoint �� Triggers P� Property P� Value �� 
TargetName �� Property �� Value N� EnterActions  	Timeline1 N� ExitActions  	Timeline2�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      !cclr-namespace:PolygonEditor  5      %L  &	        7  7  �� 5      "  ��6*    5   
   ���� 5	      	Timeline1 .��5
      U� $
 glow��60   $ (UIElement.Opacity)�6M   $ 00:00:00X�6}   .\�5      �� $ 00:00:00.3000000��6+   $ 1Q�6F   5      5      �� 5      	Timeline2 .��5      U� $
 glow��60   $ (UIElement.Opacity)�6M   $ 00:00:00X�6}   .\�5      �� $ 00:00:00.3000000��6+   $ 0Q�6F   5      5      5      .R�5      �� $ border��6   ���	#70000000���	#C0000000$��1,1,1,1q�6T   $ 4,4,4,4��6n   ._�5      � 	 5      �� 5      $��0.507* �6(   �� 5      $��0.493* �6(   .+�5       �� $
 glow��6   $��2��6+   $
 0Q�6<   $��Stretch=�6H   $
��Auto��6f   $ 4,4,4,4��6s   ��5!      
� 5"       5$   "   V� 5%   &   .~�5&   *   �� $ 1.702Q�69   $ 2.243Q�6H   �� 5'   *   $ 0Q�68   $ 0Q�6C   �� 5(   *   $ 0Q�6:   T� 5)   *   $ -0.368Q�6=   $ -0.152Q�6H   5*   '   .��5-   "   � $ 	#B28DBDFF��6/   $ 0Q�6A   � 5/   "   $ 	#008DBDFF��6/   $	 0.8Q�6A   51      53      �� 55      $��2��6'   $��Center=�68   $��Center=�6U   $
��Auto��6p   �� 57      $ shine��6   $��Stretch=�6,   $��0,0,0,0q�6J   $
��Auto��6[   $ 4,4,0,0��6h   ��58      �� 59      $ 0.450,0.028&�6.   $ 0.450,0.880&�6G   .��5:   "   � $ 	#99FFFFFF��6/   $ 0Q�6A   � 5;   "   $ 	#23FFFFFF��6/   $ 1Q�6A   5<      5>      5@      5A       5D   
   P� 5F       � �� . . �5G      �� $ border��6    � �� �	#800000006C   �� 5H      $
 glow��6    � y�$ Hidden=�6A   �� 5I      $ shine��6    �@J�Opacity$
 0.45Q�6?   5J      P� 5L       �@J�IsMouseOver .  5N      �� 5O      8��  6&    5R      �� 5S      8�� 6&   5V      5Z      5]      