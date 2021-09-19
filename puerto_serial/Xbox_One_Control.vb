Imports System.IO.Ports
Imports System.Speech.Recognition
Imports System.Runtime.InteropServices

Public Class Xbox_One_Control

    Public mensajesalida As String, mensajeentrada As String, x As Integer
    Declare Function joyGetPosEx Lib "winmm.dll" (ByVal uJoyID As Integer, ByRef pji As JOYINFOEX) As Integer

    <StructLayout(LayoutKind.Sequential)>
    Public Structure JOYINFOEX
        Public dwSize As Integer
        Public dwFlags As Integer
        Public dwXpos As Integer
        Public dwYpos As Integer
        Public dwZpos As Integer
        Public dwRpos As Integer
        Public dwUpos As Integer
        Public dwVpos As Integer
        Public dwButtons As Integer
        Public dwButtonNumber As Integer
        Public dwPOV As Integer
        Public dwReserved1 As Integer
        Public dwReserved2 As Integer
    End Structure

    Dim myjoyEX As JOYINFOEX
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button2.Enabled = False

        myjoyEX.dwSize = 64
        myjoyEX.dwFlags = &HFF ' All information
        Timer2.Interval = 10  'Update at 5 hz



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'limpia los datos del combo
        ComboBox1.Items.Clear()
        'ínicio un ciclo para cargar los puertos
        For Each puerto_disponible In My.Computer.Ports.SerialPortNames
            ComboBox1.Items.Add(puerto_disponible)
        Next
        If (ComboBox1.Items.Count > 0) Then
            ComboBox1.Text = ComboBox1.Items(0)
            Button2.Enabled = True
        Else
            MsgBox("no existe puertos disponibles")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If (Button2.Text = "Conectar") Then
            Button2.Text = "Desconectar"
            SerialPort1.PortName = ComboBox1.Text
            SerialPort1.Open()
            Timer1.Enabled = True
            Timer2.Enabled = True
        ElseIf (Button2.Text = "Desconectar") Then
            Button2.Text = "Conectar"
            SerialPort1.Close()
            Timer1.Enabled = False
            Timer2.Enabled = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)



    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim Buffer As String

        ' Get the joystick information
        Call joyGetPosEx(0, myjoyEX)

        With myjoyEX
            d1.Text = .dwXpos.ToString          'Up to six axis supported
            d2.Text = .dwYpos.ToString
            d3.Text = .dwZpos.ToString
            d4.Text = .dwRpos.ToString
            d5.Text = .dwUpos.ToString
            d7.Text = .dwButtons.ToString("X")  'Print in Hex, so can see the individual bits associated with the buttons
            d8.Text = .dwButtonNumber.ToString  'number of buttons pressed at the same time
            d9.Text = (.dwPOV / 100).ToString     'POV hat (in 1/100ths of degrees, so divided by 100 to give degrees)


            Buffer = d1.Text + ";" + d2.Text + ";" + d5.Text + ";" + d4.Text + ";" + d3.Text + ";" + d9.Text + ";" + d7.Text + ";" + d8.Text + ";"
            SerialPort1.DiscardOutBuffer()
            SerialPort1.WriteLine(Buffer)


        End With
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        '' SerialPort1.WriteLine("128;1223;3234;2321;3245;98;")

        SerialPort1.DiscardOutBuffer()
        mensajesalida = TextBox1.Text
        SerialPort1.WriteLine(mensajesalida)

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub d2_TextChanged(sender As Object, e As EventArgs) Handles d2.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        mensajeentrada = SerialPort1.ReadExisting
        TextBox3.Text = mensajeentrada

        Dim y1 As Integer, y2 As Integer
        Dim a() As String, s As String

        s = mensajeentrada
        a = Split(s, ";")


    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived

    End Sub
End Class
