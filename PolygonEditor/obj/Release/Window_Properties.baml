   M S B A M L   `   `   `  ���� H  DPolygonEditor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PolygonEditorPolygonEditor  %    PolygonEditor.Window_Properties   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     X TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  #cclr-namespace:PolygonEditor    � Title$  Application Properties�� � Icon$ images/Simple.ico��$	��377��$	��908�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=� � Topmost .  � 	Resources  �� 
TargetType �� Property �� Value��%b   '��       �� " ��.���  � ��$ 60����  � ��$ 26����  � ��$ 10,10,15,10q�.��� $
��Left=�$	��902������ 	 �� ImageSource$	 images\bg_46.jpg��
 j� Viewport$
 0,0,256,256� j� ViewportUnits$ Absolute=� j� TileMode$
 Tile=� � RowDefinitions �� $��36 ��� �� $��54 � � ColumnDefinitions �� $��20 ��� �� $��29 �.+��� -   $��Selector��$��2��$��0,14,0,0q�$��1��.@��� $��Polygon Options��.��� $��3��$��3��.+��� $��: Self-Intersection Check��$��RightToLeft=�$��Right=�$
��Left=�$��18��$��	42,21,0,0q�$	��Top=�$	��150�� d� 	IsChecked �� *SelfIntersectionCheck+�� $�� Polygon Operation ��$��28,49,28,27q�.��� .+��� $��SUBJECT:��$
��Left=�$��	52,12,0,0q�$	��Top=��� $��StrokeWeight:��$��
151,16,0,0q�$��20��$	��Top=�$
��Left=�$��78���� -   $"��Selector_SubjectStrokeWeight��$��
232,14,0,0q�|�.  $
��Left=�$��70��$��22��$	��Top=� �� SelectedIndex �� *SubjectStrokeWidth+.@���  �� 
IsSelected . .���� 
���   �	 �� X1$ 0��	 �� X2$ 40��	 �� Y1$ 10��	 �� Y2$ 10��$��20��$��1�� �� EdgeMode$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��2��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��3��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��4��$ Aliased=��� $��StrokeColor:��$��
357,16,0,0q�$��20��$	��Top=�$
��Left=�$��71��#  Xceed.Wpf.Toolkit.ColorPicker  -     Name$! Selector_SubjectStrokeColor��$��
430,14,0,0q�$��70��|�.  $��22��$	��Top=�$
��Left=�   SelectedColor �� *SubjectStrokeColor+�� $��
FillColor:��$��
558,16,0,0q�$��19��$	��Top=�$
��Left=�$��52��  -   $ Selector_SubjectFillColor��$��
612,14,0,0q�$��70��|�.  $��23��$	��Top=�$
��Left=� �� *SubjectFillColor+�� $��CLIPPER:��$
��Left=�$��	52,59,0,0q�$	��Top=��� $��StrokeWeight:��$��
151,63,0,0q�$��20��$	��Top=�$
��Left=�$��78���� -    ��Name$" Selector_ClipperStrokeWeight��$��
232,61,0,0q�|�.  $
��Left=�$��70��$��22��$	��Top=� �� *ClipperStrokeWidth+.@���  . .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��1��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��2��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��3��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��4��$ Aliased=��� $��StrokeColor:��$��
357,63,0,0q�$��20��$	��Top=�$
��Left=�$��71��  -   $! Selector_ClipperStrokeColor��$��
430,61,0,0q�$��70��|�.  $��22��$	��Top=�$
��Left=� �� *ClipperStrokeColor+�� $��
FillColor:��$��
558,63,0,0q�$��19��$	��Top=�$
��Left=�$��52��  -   $ Selector_ClipperFillColor��$��
612,61,0,0q�$��70��|�.  $��23��$	��Top=�$
��Left=� �� *ClipperFillColor+�� $��RESULT:��$
��Left=�$��
52,105,0,0q�$	��Top=��� $��StrokeWeight:��$��151,109,0,0q�$��20��$	��Top=�$
��Left=�$��78���� -   $! Selector_ResultStrokeWeight��$��232,107,0,0q�|�.  $
��Left=�$��70��$��22��$	��Top=� �� *ResultStrokeWidth+.@���  . .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��1��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��2��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��3��$ Aliased=��� .���� 
���   �$ 0��$ 40��$ 10��$ 10��$��20��$��4��$ Aliased=��� $��StrokeColor:��$��357,109,0,0q�$��20��$	��Top=�$
��Left=�$��71��  -	   $  Selector_ResultStrokeColor��$��430,107,0,0q�$��70��|�.  $��22��$	��Top=�$
��Left=� �� *ResultStrokeColor+�� $��
FillColor:��$��558,109,0,0q�$��19��$	��Top=�$
��Left=�$��52��  -
   $ Selector_ResultFillColor��$��612,107,0,0q�$��70��|�.  $��23��$	��Top=�$
��Left=� �� *ResultFillColor+�� $��Data Location��.��� $��3��$��3��.+��� $��Default files location :��$��20��$	��Top=�$
��Left=�$	��119��$��	17,15,0,0q��� $��
140,12,0,0q�{�.  $��25��$	��Top=�$
��Left=�$	��647������ *FileLocation+�� -   $	��...��$
��Left=�$��49��$	��Top=�$��
789,12,0,0q�$��25���� $�� DataBase Connection ��$��5,41,0,0q�$	��185��$	��Top=�$
��Left=�$	��834��.��� .+��� $��UserLogin :��$
��Left=�$��26��$��24,7,0,0q�$	��Top=�$��70���� $��96,9,0,0q�$��26��$	��Top=�$
��Left=�$	��253������ *	UserLogin+�� $��UserPassword :��$
��Left=�$��26��$��	399,7,0,0q�$	��Top=�$��90��G� -   $��_PasswordBox��$��	491,9,0,0q�$��26��$	��Top=�$
��Left=�$	��169��"   PolygonEditor.PasswordHelper   Attach .    Password �� *Text+ �� ElementName$ _PasswordTextBox�� �� Mode$ TwoWay=�y���  �� Path$ 	IsChecked�$ ShowPassword�� '  #BooleanToVisibilityConverterInverse �� 	Converter# [  �� -   $��_PasswordTextBox��$��	491,9,0,0q�$��26��$	��Top=�$
��Left=�$	��169��y��� $ 	IsChecked�$ ShowPassword��   BooleanToVisibilityConverter# [ �� -   $��ShowPassword��$��Show password��$��Right=�$
��Left=�$ FalseY�$��17��$��
675,13,0,0q�$	��Top=�$	��103���� $��
RemoteHost��$��54��$��	20,41,0,0q�$	��Top=�$
��Left=�$	��783��  J� RenderTransformOrigin$  0.501,0.537&�.��� .+��� $��Used :��$
��Left=�$	��Top=�$��13,3,0,0q�	� -   $��RadioButton_RemoteHost��! 	� 	GroupName$! 
HostSelect��$
��Left=�$��	60,11,0,0q�$	��Top=��� $��	Address :��$
��Left=�$	��Top=�$��	103,3,0,0q�{��� $ RadioButton_RemoteHost��$ 	IsChecked��� $��	170,3,0,0q�$��26��$	��Top=�$
��Left=�$	��253������ *RemoteHostAddress+{��� $ RadioButton_RemoteHost��$ 	IsChecked��� $��Port :��$
��Left=�$	��Top=�$��	459,3,0,0q�{��� $ RadioButton_RemoteHost��$ 	IsChecked��� $��	501,3,0,0q�$��26��$	��Top=�$
��Left=�$	��253������ *
RemotePort+{��� $ RadioButton_RemoteHost��$ 	IsChecked��� $��	LocalHost��$��54��$��
20,101,0,0q�$	��Top=�$
��Left=�$	��783��$  0.501,0.537&�.��� .+��� $��Used :��$
��Left=�$	��Top=�$��13,3,0,0q�	� -   $��RadioButton_LocalHost��$! 
HostSelect��$
��Left=�$��	60,11,0,0q�$	��Top=��� $��	Address :��$
��Left=�$	��Top=�$��	103,3,0,0q�{��� $ RadioButton_LocalHost��$ 	IsChecked��� $��	170,3,0,0q�$��26��$	��Top=�$
��Left=�$	��253������ *LocalHostAddress+{��� $ RadioButton_LocalHost��$ 	IsChecked��� $��Port :��$
��Left=�$	��Top=�$��	459,3,0,0q�{��� $ RadioButton_LocalHost��$ 	IsChecked��� $��	501,3,0,0q�$��26��$	��Top=�$
��Left=�$	��253������ *	LocalPort+{��� $ RadioButton_LocalHost��$ 	IsChecked��� " �� Orientation$" 
Horizontal=�$��Right=�$	��160��$��1��$��2��$��0,0,3,7q�.+��� -   # �� 	IsDefault# . .��Save�� $ �� IsCancel$ . .��Cancel