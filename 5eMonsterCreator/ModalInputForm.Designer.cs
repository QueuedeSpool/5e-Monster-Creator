namespace _5eMonsterCreator
{
    partial class ModalInputForm
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
            this.valueLabel = new System.Windows.Forms.Label();
            this.modalInputUpDown = new System.Windows.Forms.NumericUpDown();
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.modalInputUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(12, 9);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(37, 13);
            this.valueLabel.TabIndex = 0;
            this.valueLabel.Text = "Value:";
            // 
            // modalInputUpDown
            // 
            this.modalInputUpDown.DecimalPlaces = 2;
            this.modalInputUpDown.Location = new System.Drawing.Point(15, 25);
            this.modalInputUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.modalInputUpDown.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.modalInputUpDown.Name = "modalInputUpDown";
            this.modalInputUpDown.Size = new System.Drawing.Size(247, 20);
            this.modalInputUpDown.TabIndex = 1;
            this.modalInputUpDown.Enter += new System.EventHandler(this.modalInputUpDown_Enter);
            this.modalInputUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.modalInputUpDown_KeyDown);
            this.modalInputUpDown.KeyUp += new System.Windows.Forms.KeyEventHandler(this.modalInputUpDown_KeyUp);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(15, 51);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(247, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // ModalInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 92);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.modalInputUpDown);
            this.Controls.Add(this.valueLabel);
            this.Name = "ModalInputForm";
            this.Text = "ModalInputForm";
            ((System.ComponentModel.ISupportInitialize)(this.modalInputUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.NumericUpDown modalInputUpDown;
        private System.Windows.Forms.Button okButton;
    }
}