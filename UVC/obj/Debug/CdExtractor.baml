   M S B A M L   `   `   `  ����  UVC!    AudioCDExtractor.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Audio CD Extractor��5   	   $	��450��6$   $	��800��61   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   �� -   5         ��Name$ DriveComboBox��5      $��0��6*   $��0,0,0,10q�67   5      �� 5   
   -   5         ��Name$ TrackListBox��5      $��1��6(   $��0,0,0,10q�65   ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $��0,0,10,0q�6A   ��6$   �� *TrackNumber+6A   �� 5      ��6$   �� *
Duration+5      5      5      �� 5   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5      �� -   5         ��Name$ OutputPathTextBox��5      $	��300��61   $��0,0,10,0q�6=   $��	C:\Output��5      �� 5      -   5        $��Browse��5      $��0,0,10,0q�6A   �� 5      -   5        $��Extract Selected��5      60   5      5       5!      