   M S B A M L   `   `   `  ����Y  UPassengerTransportationProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:PassengerTransportationProjectPassengerTransportationProject  5   	   /    )PassengerTransportationProject.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   8local,clr-namespace:PassengerTransportationProject  5   	   -   5        $��MainWin��5	   [   $	��600��6	   $
��1200��6     � WindowStyle$
  None=�6#    � WindowStartupLocation$ CenterScreen=�66    � 
ResizeMode$ NoResize=�6�   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��6* �6   .+�5   
   � .+�5      �� -   5         ��Name$ CollapseButton��5   #    � Cursor$
 Hand��6   $��35��6;   $��40��6G   $	��Top=�6R   $��Right=�6j      MaterialDesignToolButton#��[  6�   $��0 0 40 0q�6�   .��5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Minus=�6*   
�������67   5      �� 5      -   5        $ CloseButton��5   #   $
 Hand��6   $��35��68   $��40��6D   $	��Top=�6P   $��Right=�6h   #��[  6�   .��5        $ WindowClose=�6*   
���\\��6=   5      5      � 5   
   $��1��6   .+�5       � -   5        $��	MainFrame��5        � NavigationUIVisibility$ Hidden=�6%   5      � 5   
   -   5         �Name$ GridBar��5      $
��auto��6    $��Center=�6-   $��0��6H   .+�5      ]� ��.  .+�5      �� -   5        $��MainPageButton��5   �   	 � ToolTip$+	 %На главную страницу.��6   $	��130��69   $��47��6E   
�������$
��10 0q�6e   ���#279667.��5      �� 
 �� Orientation$
 
Horizontal=�6!   .+�5        $
��Left=�62   $��Center=�6M   $ DotsGrid=�6h   $��30��6x   $��30��6�   $	��3 0q�6�   �� 5      $��16(�6$    �� TextAlignment$ Center=�62   $��Center=�6I   $��Center=�6f   Билеты6�   5       5!      �� 5"      $
 
Horizontal=�6   .+�5#      �� -   5        $��AuthtorizationButton��5#   0   
�������$
 Hand��6L   $
��20 0q�6�    � ContextMenu 5$      �� 5%      -   5        $��ContextMenuUser��5%   *   $	��250��6A   $��75��6M   $y�	Collapsed=�6Y   .@�5&   "   m� -	   5        $��
ExitButton��5&   +   $��
Выйти��6V    m� Icon 5'   &     5(   *   $ 	ExitToApp=�6B   5*   #   5+      ._�5-      �� -
   5        $��MyProfileButton��5-   4   
�������$
 Hand��6�   ._�5.      �� $
 Vertical=�6)   $��Right=�6A   $��Center=�6]   .+�5/   "     $ AccountOutline=�6;   $��Center=�6Q   $��30��6n   $��30��6y   
�������6�   �� 50   "   -   5         ��Name$ NameUser��50   ,   $y�	Collapsed=�6>   $��13(�6U   $��SemiBold"�6c   $ Center=�6y   �� 51   "   -   5        $ Authtorization��51   ,   $��Профиль��6D   $��13(�6S   $��SemiBold"�6a   $ Center=�6w   52      53      54      �� 55      -   5        $��MyTicketsButton��55   0   
�������$
 Hand��6�   $
��20 0q�6�   ._�56      �� $
 Vertical=�6%   $��Right=�6<   $��Center=�6X   .+�57        $ TicketConfirmationOutline=�66   $��Center=�6W   $��30��6t   $��30��6   
�������6�   �� 58      $��Мои билеты��6(   $��13(�6:   $��SemiBold"�6H   $ Center=�6^   59      5:      5;      5<      5=      5>      5?      