namespace TestApp
{
    partial class Form1
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
            this.bServerStart = new System.Windows.Forms.Button();
            this.bServerEnd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lClient = new System.Windows.Forms.Label();
            this.bClientStart = new System.Windows.Forms.Button();
            this.bClientEnd = new System.Windows.Forms.Button();
            this.bSendAll = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.bSendOne = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bServerStart
            // 
            this.bServerStart.Location = new System.Drawing.Point(57, 8);
            this.bServerStart.Name = "bServerStart";
            this.bServerStart.Size = new System.Drawing.Size(75, 23);
            this.bServerStart.TabIndex = 0;
            this.bServerStart.Text = "Start";
            this.bServerStart.UseVisualStyleBackColor = true;
            this.bServerStart.Click += new System.EventHandler(this.bServerStart_Click);
            // 
            // bServerEnd
            // 
            this.bServerEnd.Location = new System.Drawing.Point(138, 8);
            this.bServerEnd.Name = "bServerEnd";
            this.bServerEnd.Size = new System.Drawing.Size(75, 23);
            this.bServerEnd.TabIndex = 1;
            this.bServerEnd.Text = "End";
            this.bServerEnd.UseVisualStyleBackColor = true;
            this.bServerEnd.Click += new System.EventHandler(this.bServerEnd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server";
            // 
            // lClient
            // 
            this.lClient.AutoSize = true;
            this.lClient.Location = new System.Drawing.Point(16, 42);
            this.lClient.Name = "lClient";
            this.lClient.Size = new System.Drawing.Size(33, 13);
            this.lClient.TabIndex = 7;
            this.lClient.Text = "Client";
            // 
            // bClientStart
            // 
            this.bClientStart.Location = new System.Drawing.Point(57, 37);
            this.bClientStart.Name = "bClientStart";
            this.bClientStart.Size = new System.Drawing.Size(75, 23);
            this.bClientStart.TabIndex = 8;
            this.bClientStart.Text = "Start";
            this.bClientStart.UseVisualStyleBackColor = true;
            this.bClientStart.Click += new System.EventHandler(this.bClientStart_Click);
            // 
            // bClientEnd
            // 
            this.bClientEnd.Location = new System.Drawing.Point(138, 37);
            this.bClientEnd.Name = "bClientEnd";
            this.bClientEnd.Size = new System.Drawing.Size(75, 23);
            this.bClientEnd.TabIndex = 9;
            this.bClientEnd.Text = "End";
            this.bClientEnd.UseVisualStyleBackColor = true;
            this.bClientEnd.Click += new System.EventHandler(this.bClientEnd_Click);
            // 
            // bSendAll
            // 
            this.bSendAll.Location = new System.Drawing.Point(219, 8);
            this.bSendAll.Name = "bSendAll";
            this.bSendAll.Size = new System.Drawing.Size(75, 23);
            this.bSendAll.TabIndex = 10;
            this.bSendAll.Text = "Send All";
            this.bSendAll.UseVisualStyleBackColor = true;
            this.bSendAll.Click += new System.EventHandler(this.bSendAll_Click);
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(219, 37);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(75, 23);
            this.bSend.TabIndex = 11;
            this.bSend.Text = "Send";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // bSendOne
            // 
            this.bSendOne.Location = new System.Drawing.Point(300, 8);
            this.bSendOne.Name = "bSendOne";
            this.bSendOne.Size = new System.Drawing.Size(75, 23);
            this.bSendOne.TabIndex = 12;
            this.bSendOne.Text = "Send One";
            this.bSendOne.UseVisualStyleBackColor = true;
            this.bSendOne.Click += new System.EventHandler(this.bSendOne_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 80);
            this.Controls.Add(this.bSendOne);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.bSendAll);
            this.Controls.Add(this.bClientEnd);
            this.Controls.Add(this.bClientStart);
            this.Controls.Add(this.lClient);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bServerEnd);
            this.Controls.Add(this.bServerStart);
            this.Name = "Form1";
            this.Text = "Test Connection App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bServerStart;
        private System.Windows.Forms.Button bServerEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lClient;
        private System.Windows.Forms.Button bClientStart;
        private System.Windows.Forms.Button bClientEnd;
        private System.Windows.Forms.Button bSendAll;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bSendOne;
    }
}

