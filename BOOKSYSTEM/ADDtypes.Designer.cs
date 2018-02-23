namespace BOOKSYSTEM
{
    partial class ADDtypes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADDtypes));
            this.addtypestextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.submitbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addtypestextBox
            // 
            this.addtypestextBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.addtypestextBox.Location = new System.Drawing.Point(40, 57);
            this.addtypestextBox.Name = "addtypestextBox";
            this.addtypestextBox.Size = new System.Drawing.Size(170, 26);
            this.addtypestextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ADD book types:";
            // 
            // submitbutton
            // 
            this.submitbutton.Location = new System.Drawing.Point(147, 122);
            this.submitbutton.Name = "submitbutton";
            this.submitbutton.Size = new System.Drawing.Size(75, 32);
            this.submitbutton.TabIndex = 2;
            this.submitbutton.Text = "submit";
            this.submitbutton.UseVisualStyleBackColor = true;
            this.submitbutton.Click += new System.EventHandler(this.submitbutton_Click);
            // 
            // ADDtypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 237);
            this.Controls.Add(this.submitbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addtypestextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ADDtypes";
            this.Text = "ADDtypes";
            this.Load += new System.EventHandler(this.ADDtypes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox addtypestextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button submitbutton;
    }
}