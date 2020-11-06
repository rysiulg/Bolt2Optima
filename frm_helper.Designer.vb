<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_helper
    Inherits System.Windows.Forms.Form

    'Formularz przesłania metodę dispose, aby wyczyścić listę składników.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wymagane przez Projektanta formularzy systemu Windows
    Private components As System.ComponentModel.IContainer

    'UWAGA: następująca procedura jest wymagana przez Projektanta formularzy systemu Windows
    'Możesz to modyfikować, używając Projektanta formularzy systemu Windows. 
    'Nie należy modyfikować za pomocą edytora kodu.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pict_helper = New System.Windows.Forms.PictureBox()
        CType(Me.pict_helper, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pict_helper
        '
        Me.pict_helper.BackColor = System.Drawing.SystemColors.ControlLight
        Me.pict_helper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pict_helper.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pict_helper.Image = Global.Bolt2Optima.My.Resources.Resources.optima_konfiguracja_rozproszona
        Me.pict_helper.Location = New System.Drawing.Point(0, 0)
        Me.pict_helper.Margin = New System.Windows.Forms.Padding(4)
        Me.pict_helper.Name = "pict_helper"
        Me.pict_helper.Size = New System.Drawing.Size(784, 561)
        Me.pict_helper.TabIndex = 0
        Me.pict_helper.TabStop = False
        '
        'frm_helper
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.pict_helper)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ForeColor = System.Drawing.Color.Red
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frm_helper"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Konfiguracja IDKsieg w Optimie"
        CType(Me.pict_helper, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pict_helper As PictureBox
End Class
