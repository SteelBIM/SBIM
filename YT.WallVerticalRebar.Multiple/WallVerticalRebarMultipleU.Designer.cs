
namespace YT.WallVerticalRebar.Multiple
{
    partial class WallVerticalRebarMultipleU
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
            this.tboxText = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tboxText
            // 
            this.structuresExtender.SetAttributeName(this.tboxText, "FilePath");
            this.structuresExtender.SetAttributeTypeName(this.tboxText, "String");
            this.structuresExtender.SetBindPropertyName(this.tboxText, "Text");
            this.tboxText.Location = new System.Drawing.Point(12, 12);
            this.tboxText.Name = "tboxText";
            this.tboxText.Size = new System.Drawing.Size(294, 21);
            this.tboxText.TabIndex = 0;
            // 
            // btnOpen
            // 
            this.structuresExtender.SetAttributeName(this.btnOpen, null);
            this.structuresExtender.SetAttributeTypeName(this.btnOpen, null);
            this.structuresExtender.SetBindPropertyName(this.btnOpen, null);
            this.btnOpen.Location = new System.Drawing.Point(231, 39);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "FilePath";
            this.btnOpen.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.structuresExtender.SetAttributeName(this.btnRun, null);
            this.structuresExtender.SetAttributeTypeName(this.btnRun, null);
            this.structuresExtender.SetBindPropertyName(this.btnRun, null);
            this.btnRun.Location = new System.Drawing.Point(312, 39);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            // 
            // WallVerticalRebarMultipleU
            // 
            this.structuresExtender.SetAttributeName(this, null);
            this.structuresExtender.SetAttributeTypeName(this, null);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.structuresExtender.SetBindPropertyName(this, null);
            this.ClientSize = new System.Drawing.Size(435, 77);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.tboxText);
            this.Name = "WallVerticalRebarMultipleU";
            this.Text = "WallVerticalRebarMultipleU";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tboxText;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnRun;
    }
}