<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ChangelogInfo
    Inherits System.Windows.Forms.Form

    'Formularz przesłania metodę dispose, aby wyczyścić listę składników.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChangelogInfo))
        Me.btn_OK = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.InfoBox_details = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'btn_OK
        '
        Me.btn_OK.Location = New System.Drawing.Point(679, 12)
        Me.btn_OK.Name = "btn_OK"
        Me.btn_OK.Size = New System.Drawing.Size(93, 24)
        Me.btn_OK.TabIndex = 0
        Me.btn_OK.Text = "Zamknij"
        Me.btn_OK.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(12, 12)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(324, 21)
        Me.ComboBox1.Sorted = True
        Me.ComboBox1.TabIndex = 1
        '
        'InfoBox_details
        '
        Me.InfoBox_details.CausesValidation = False
        Me.InfoBox_details.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.InfoBox_details.Location = New System.Drawing.Point(0, 42)
        Me.InfoBox_details.Name = "InfoBox_details"
        Me.InfoBox_details.ReadOnly = True
        Me.InfoBox_details.Size = New System.Drawing.Size(784, 588)
        Me.InfoBox_details.TabIndex = 6
        Me.InfoBox_details.Text = ""
        '
        'ChangelogInfo
        '
        Me.AcceptButton = Me.btn_OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 630)
        Me.Controls.Add(Me.InfoBox_details)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.btn_OK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "ChangelogInfo"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bolt2Optima InfoBox (c) MARM.pl"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btn_OK As Button
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents InfoBox_details As RichTextBox
End Class
