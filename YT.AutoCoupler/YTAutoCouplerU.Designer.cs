
namespace YT.AutoCoupler
{
    partial class YTAutoCouplerU
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveLoad1 = new Tekla.Structures.Dialog.UIControls.SaveLoad();
            this.okApplyModifyGetOnOffCancel1 = new Tekla.Structures.Dialog.UIControls.OkApplyModifyGetOnOffCancel();
            this.CouplerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.CouplerClass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CouplerPosition = new System.Windows.Forms.ComboBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox176 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveLoad1
            // 
            this.structuresExtender.SetAttributeName(this.saveLoad1, null);
            this.structuresExtender.SetAttributeTypeName(this.saveLoad1, null);
            this.saveLoad1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.structuresExtender.SetBindPropertyName(this.saveLoad1, null);
            this.saveLoad1.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveLoad1.HelpFileType = Tekla.Structures.Dialog.UIControls.SaveLoad.HelpFileTypeEnum.General;
            this.saveLoad1.HelpKeyword = "";
            this.saveLoad1.HelpUrl = "";
            this.saveLoad1.Location = new System.Drawing.Point(0, 0);
            this.saveLoad1.Name = "saveLoad1";
            this.saveLoad1.SaveAsText = "";
            this.saveLoad1.Size = new System.Drawing.Size(680, 40);
            this.saveLoad1.TabIndex = 0;
            this.saveLoad1.UserDefinedHelpFilePath = null;
            // 
            // okApplyModifyGetOnOffCancel1
            // 
            this.structuresExtender.SetAttributeName(this.okApplyModifyGetOnOffCancel1, null);
            this.structuresExtender.SetAttributeTypeName(this.okApplyModifyGetOnOffCancel1, null);
            this.structuresExtender.SetBindPropertyName(this.okApplyModifyGetOnOffCancel1, null);
            this.okApplyModifyGetOnOffCancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.okApplyModifyGetOnOffCancel1.Location = new System.Drawing.Point(0, 122);
            this.okApplyModifyGetOnOffCancel1.Name = "okApplyModifyGetOnOffCancel1";
            this.okApplyModifyGetOnOffCancel1.Size = new System.Drawing.Size(680, 27);
            this.okApplyModifyGetOnOffCancel1.TabIndex = 1;
            // 
            // CouplerName
            // 
            this.structuresExtender.SetAttributeName(this.CouplerName, "CouplerName");
            this.structuresExtender.SetAttributeTypeName(this.CouplerName, "String");
            this.structuresExtender.SetBindPropertyName(this.CouplerName, "Text");
            this.CouplerName.Location = new System.Drawing.Point(27, 38);
            this.CouplerName.Name = "CouplerName";
            this.CouplerName.Size = new System.Drawing.Size(72, 21);
            this.CouplerName.TabIndex = 4;
            // 
            // label1
            // 
            this.structuresExtender.SetAttributeName(this.label1, null);
            this.structuresExtender.SetAttributeTypeName(this.label1, null);
            this.label1.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label1, null);
            this.label1.Location = new System.Drawing.Point(45, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "이름";
            // 
            // groupBox1
            // 
            this.structuresExtender.SetAttributeName(this.groupBox1, null);
            this.structuresExtender.SetAttributeTypeName(this.groupBox1, null);
            this.structuresExtender.SetBindPropertyName(this.groupBox1, null);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.CouplerClass);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.CouplerPosition);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.checkBox176);
            this.groupBox1.Controls.Add(this.CouplerName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(656, 65);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "주근커플러";
            // 
            // label6
            // 
            this.structuresExtender.SetAttributeName(this.label6, null);
            this.structuresExtender.SetAttributeTypeName(this.label6, null);
            this.label6.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label6, null);
            this.label6.Location = new System.Drawing.Point(147, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "클래스";
            // 
            // checkBox5
            // 
            this.structuresExtender.SetAttributeName(this.checkBox5, "CouplerClass");
            this.structuresExtender.SetAttributeTypeName(this.checkBox5, null);
            this.checkBox5.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.checkBox5, "Checked");
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.structuresExtender.SetIsFilter(this.checkBox5, true);
            this.checkBox5.Location = new System.Drawing.Point(123, 41);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(15, 14);
            this.checkBox5.TabIndex = 43;
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // CouplerClass
            // 
            this.structuresExtender.SetAttributeName(this.CouplerClass, "CouplerClass");
            this.structuresExtender.SetAttributeTypeName(this.CouplerClass, "String");
            this.structuresExtender.SetBindPropertyName(this.CouplerClass, "Text");
            this.CouplerClass.Location = new System.Drawing.Point(144, 38);
            this.CouplerClass.Name = "CouplerClass";
            this.CouplerClass.Size = new System.Drawing.Size(49, 21);
            this.CouplerClass.TabIndex = 42;
            // 
            // label3
            // 
            this.structuresExtender.SetAttributeName(this.label3, null);
            this.structuresExtender.SetAttributeTypeName(this.label3, null);
            this.label3.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.label3, null);
            this.label3.Location = new System.Drawing.Point(248, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 41;
            this.label3.Text = "위치";
            // 
            // CouplerPosition
            // 
            this.structuresExtender.SetAttributeName(this.CouplerPosition, "CouplerPosition");
            this.structuresExtender.SetAttributeTypeName(this.CouplerPosition, "String");
            this.structuresExtender.SetBindPropertyName(this.CouplerPosition, "Text");
            this.CouplerPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CouplerPosition.FormattingEnabled = true;
            this.CouplerPosition.Items.AddRange(new object[] {
            "모두",
            "상",
            "하"});
            this.CouplerPosition.Location = new System.Drawing.Point(239, 38);
            this.CouplerPosition.Name = "CouplerPosition";
            this.CouplerPosition.Size = new System.Drawing.Size(62, 20);
            this.CouplerPosition.TabIndex = 40;
            // 
            // checkBox4
            // 
            this.structuresExtender.SetAttributeName(this.checkBox4, "CouplerPosition");
            this.structuresExtender.SetAttributeTypeName(this.checkBox4, null);
            this.checkBox4.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.checkBox4, "Checked");
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.structuresExtender.SetIsFilter(this.checkBox4, true);
            this.checkBox4.Location = new System.Drawing.Point(218, 41);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(15, 14);
            this.checkBox4.TabIndex = 39;
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox176
            // 
            this.structuresExtender.SetAttributeName(this.checkBox176, "CouplerName");
            this.structuresExtender.SetAttributeTypeName(this.checkBox176, null);
            this.checkBox176.AutoSize = true;
            this.structuresExtender.SetBindPropertyName(this.checkBox176, "Checked");
            this.checkBox176.Checked = true;
            this.checkBox176.CheckState = System.Windows.Forms.CheckState.Checked;
            this.structuresExtender.SetIsFilter(this.checkBox176, true);
            this.checkBox176.Location = new System.Drawing.Point(10, 41);
            this.checkBox176.Name = "checkBox176";
            this.checkBox176.Size = new System.Drawing.Size(15, 14);
            this.checkBox176.TabIndex = 35;
            this.checkBox176.UseVisualStyleBackColor = true;
            // 
            // YTAutoCouplerU
            // 
            this.structuresExtender.SetAttributeName(this, null);
            this.structuresExtender.SetAttributeTypeName(this, null);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.structuresExtender.SetBindPropertyName(this, null);
            this.ClientSize = new System.Drawing.Size(680, 149);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okApplyModifyGetOnOffCancel1);
            this.Controls.Add(this.saveLoad1);
            this.Name = "YTAutoCouplerU";
            this.Text = "YTAutoCoupler";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Tekla.Structures.Dialog.UIControls.SaveLoad saveLoad1;
        private Tekla.Structures.Dialog.UIControls.OkApplyModifyGetOnOffCancel okApplyModifyGetOnOffCancel1;
        private System.Windows.Forms.TextBox CouplerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox176;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CouplerPosition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.TextBox CouplerClass;
    }
}