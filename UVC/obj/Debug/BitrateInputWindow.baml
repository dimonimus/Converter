   M S B A M L   `   `   `  ����  UVC    UVC.BitrateInputWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$#  Введите битрейт��5   	   $	��300��6!   $	��300��6.    � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�6   
������  � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   	   .��5
      ��  �� CornerRadius$ 12��6   $��2q�6   
���   ���5   
   �� 5       �� Color$ White��6   ._�5   
   �� $ 10��6   $��2q�6#    J� Opacity$
 0.95Q�67   ��5      �� 5       �� 
StartPoint$	 0,0&�6&   	 �� EndPoint$		 1,1&�67   .��5      � 
 � Color$
 #FF8C00��6#    � Offset$ 0Q�63   � 5      $
 #FFA500��6#   $
 0.75Q�63   � 5      $
 #FFB347��6#   $ 1Q�63   5      ��5      �� 5      $	 0,1&�6&   $		 1,0&�67   .��5      � $
 #331A00��6#   $ 0Q�63   � 5      $
 #663300��6#   $ 1Q�63   5      ._�5      �� $��10q�6   .+�5      �� $7��1Выберите качество (360p - 1080p):��6   
�������$��Copperplate Gothic Bold)�6Z   $��12(�6   �� 5      -   5        $��QualitySlider��5       � Minimum$ 1Q�6.    � Maximum$ 3Q�6:    �� TickFrequency$ 1Q�6F    �� IsSnapToTickEnabled . 5    4   �� 5!      -   5        $��QualityLabel��5!      $
��720p��6,   
�������$��Copperplate Gothic Bold)�6Q   $��12(�6v   �� 5#      $+��%Введите битрейт (kbps):��6   
�������$��Copperplate Gothic Bold)�6P   $��12(�6u   �� 5$      -   5        $��BitrateTextBox��5$      $��0,5,0,10q�60   
�������
���   �$��1q�5&      
�������6.   �� 5(      $"��Выберите кодек:��6   
�������$��Copperplate Gothic Bold)�6H   $��12(�6m   �� 5)      -   5        $��CodecComboBox��5)      $��0,5,0,10q�60   
�������
���   �$��1q�5+      
�������5,      �� 5.       �� Orientation$ 
Horizontal=�6   $��Right=�66   .+�5/      �� -   5        $��OkButton��5/   *   $��OK��6   $��5,0,0,0q�50      ���#FF8C00
�������$��Copperplate Gothic Bold)�51   5    � Cursor$
 Hand��52      $	��100��6   $��40��6'   ��53      �� 54       �� 
TargetType" ��6$   .�55   "   ��  �� Property � �� �� Value �#FF8C006?    �� Triggers 56   "   P� 57   &    P� Property �@J�IsMouseOver P� Value . . �58   *   ��  � �� �#FFA5006G   59   '   5;      ��5=      �� 5>       �� 
TargetType" ��6.   .R�5?   "   �� $	��100��6)   $��40��65   $ 20��6A   #��z��6S   ._�5@   &   �� $��Center=�67   $��Center=�6R   5A   #   5B      5D      �� 5F      -   5        $��CancelButton��5F   .   $��Отмена��6   $��5,0,0,0q�5G      ���#FF4500
�������$��Copperplate Gothic Bold)�5H   5   $
 Hand��5I      $	��100��6   $��40��6'   ��5J      �� 5K      " ��6$   .�5L   "   ��  � �� �#FF45006?    5M   "   P� 5N   &    �@J�IsMouseOver . . �5O   *   ��  � �� �#FF63476G   5P   '   5R      ��5T      �� 5U      " ��6.   .R�5V   "   �� $	��100��6)   $��40��65   $ 20��6A   #��z��6S   ._�5W   &   �� $��Center=�67   $��Center=�6R   5X   #   5Y      5[      5\      5^      5_      5`      5a      