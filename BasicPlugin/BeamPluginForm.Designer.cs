
namespace BasicPlugin
{
    partial class BeamPluginForm
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
            this.okApplyModifyGetOnOffCancel1 = new Tekla.Structures.Dialog.UIControls.OkApplyModifyGetOnOffCancel();
            this.saveLoad1 = new Tekla.Structures.Dialog.UIControls.SaveLoad();
            this.BName = new System.Windows.Forms.TextBox();
            this.BProfile = new System.Windows.Forms.TextBox();
            this.BClass = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okApplyModifyGetOnOffCancel1
            // 
            this.structuresExtender.SetAttributeName(this.okApplyModifyGetOnOffCancel1, null);
            this.structuresExtender.SetAttributeTypeName(this.okApplyModifyGetOnOffCancel1, null);
            this.structuresExtender.SetBindPropertyName(this.okApplyModifyGetOnOffCancel1, null);
            this.okApplyModifyGetOnOffCancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.okApplyModifyGetOnOffCancel1.Location = new System.Drawing.Point(0, 423);
            this.okApplyModifyGetOnOffCancel1.Name = "okApplyModifyGetOnOffCancel1";
            this.okApplyModifyGetOnOffCancel1.Size = new System.Drawing.Size(800, 27);
            this.okApplyModifyGetOnOffCancel1.TabIndex = 0;
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
            this.saveLoad1.Size = new System.Drawing.Size(800, 39);
            this.saveLoad1.TabIndex = 1;
            this.saveLoad1.UserDefinedHelpFilePath = null;
            // 
            // BName
            // 
            this.structuresExtender.SetAttributeName(this.BName, "BName");
            this.structuresExtender.SetAttributeTypeName(this.BName, "String");
            this.structuresExtender.SetBindPropertyName(this.BName, "Text");
            this.BName.Location = new System.Drawing.Point(243, 128);
            this.BName.Name = "BName";
            this.BName.Size = new System.Drawing.Size(192, 21);
            this.BName.TabIndex = 2;
            // 
            // BProfile
            // 
            this.structuresExtender.SetAttributeName(this.BProfile, "BProfile");
            this.structuresExtender.SetAttributeTypeName(this.BProfile, "String");
            this.structuresExtender.SetBindPropertyName(this.BProfile, "Text");
            this.BProfile.Location = new System.Drawing.Point(243, 166);
            this.BProfile.Name = "BProfile";
            this.BProfile.Size = new System.Drawing.Size(192, 21);
            this.BProfile.TabIndex = 3;
            // 
            // BClass
            // 
            this.structuresExtender.SetAttributeName(this.BClass, "BClass");
            this.structuresExtender.SetAttributeTypeName(this.BClass, "String");
            this.structuresExtender.SetBindPropertyName(this.BClass, "Text");
            this.BClass.Location = new System.Drawing.Point(243, 214);
            this.BClass.Name = "BClass";
            this.BClass.Size = new System.Drawing.Size(192, 21);
            this.BClass.TabIndex = 4;
            // 
            // BeamPluginForm
            // 
            this.structuresExtender.SetAttributeName(this, null);
            this.structuresExtender.SetAttributeTypeName(this, null);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.structuresExtender.SetBindPropertyName(this, null);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BClass);
            this.Controls.Add(this.BProfile);
            this.Controls.Add(this.BName);
            this.Controls.Add(this.saveLoad1);
            this.Controls.Add(this.okApplyModifyGetOnOffCancel1);
            this.Name = "BeamPluginForm";
            this.Text = "BeamPluginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Tekla.Structures.Dialog.UIControls.OkApplyModifyGetOnOffCancel okApplyModifyGetOnOffCancel1;
        private Tekla.Structures.Dialog.UIControls.SaveLoad saveLoad1;
        private System.Windows.Forms.TextBox BName;
        private System.Windows.Forms.TextBox BProfile;
        private System.Windows.Forms.TextBox BClass;
    }
}