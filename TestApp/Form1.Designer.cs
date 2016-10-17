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
            this.bClientStart = new System.Windows.Forms.Button();
            this.bClientEnd = new System.Windows.Forms.Button();
            this.bServerSendAll = new System.Windows.Forms.Button();
            this.bClientSend = new System.Windows.Forms.Button();
            this.bServerSendOne = new System.Windows.Forms.Button();
            this.gbServer = new System.Windows.Forms.GroupBox();
            this.gbClient = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbServerData = new System.Windows.Forms.TextBox();
            this.tbClientData = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbServer.SuspendLayout();
            this.gbClient.SuspendLayout();
            this.SuspendLayout();
            // 
            // bServerStart
            // 
            this.bServerStart.Location = new System.Drawing.Point(6, 19);
            this.bServerStart.Name = "bServerStart";
            this.bServerStart.Size = new System.Drawing.Size(75, 23);
            this.bServerStart.TabIndex = 0;
            this.bServerStart.Text = "Start";
            this.bServerStart.UseVisualStyleBackColor = true;
            this.bServerStart.Click += new System.EventHandler(this.bServerStart_Click);
            // 
            // bServerEnd
            // 
            this.bServerEnd.Enabled = false;
            this.bServerEnd.Location = new System.Drawing.Point(87, 19);
            this.bServerEnd.Name = "bServerEnd";
            this.bServerEnd.Size = new System.Drawing.Size(75, 23);
            this.bServerEnd.TabIndex = 1;
            this.bServerEnd.Text = "End";
            this.bServerEnd.UseVisualStyleBackColor = true;
            this.bServerEnd.Click += new System.EventHandler(this.bServerEnd_Click);
            // 
            // bClientStart
            // 
            this.bClientStart.Location = new System.Drawing.Point(6, 19);
            this.bClientStart.Name = "bClientStart";
            this.bClientStart.Size = new System.Drawing.Size(75, 23);
            this.bClientStart.TabIndex = 8;
            this.bClientStart.Text = "Start";
            this.bClientStart.UseVisualStyleBackColor = true;
            this.bClientStart.Click += new System.EventHandler(this.bClientStart_Click);
            // 
            // bClientEnd
            // 
            this.bClientEnd.Enabled = false;
            this.bClientEnd.Location = new System.Drawing.Point(87, 19);
            this.bClientEnd.Name = "bClientEnd";
            this.bClientEnd.Size = new System.Drawing.Size(75, 23);
            this.bClientEnd.TabIndex = 9;
            this.bClientEnd.Text = "End";
            this.bClientEnd.UseVisualStyleBackColor = true;
            this.bClientEnd.Click += new System.EventHandler(this.bClientEnd_Click);
            // 
            // bServerSendAll
            // 
            this.bServerSendAll.Enabled = false;
            this.bServerSendAll.Location = new System.Drawing.Point(168, 19);
            this.bServerSendAll.Name = "bServerSendAll";
            this.bServerSendAll.Size = new System.Drawing.Size(75, 23);
            this.bServerSendAll.TabIndex = 10;
            this.bServerSendAll.Text = "Send All";
            this.bServerSendAll.UseVisualStyleBackColor = true;
            this.bServerSendAll.Click += new System.EventHandler(this.bServerSendAll_Click);
            // 
            // bClientSend
            // 
            this.bClientSend.Enabled = false;
            this.bClientSend.Location = new System.Drawing.Point(168, 19);
            this.bClientSend.Name = "bClientSend";
            this.bClientSend.Size = new System.Drawing.Size(75, 23);
            this.bClientSend.TabIndex = 11;
            this.bClientSend.Text = "Send";
            this.bClientSend.UseVisualStyleBackColor = true;
            this.bClientSend.Click += new System.EventHandler(this.bClientSend_Click);
            // 
            // bServerSendOne
            // 
            this.bServerSendOne.Enabled = false;
            this.bServerSendOne.Location = new System.Drawing.Point(249, 19);
            this.bServerSendOne.Name = "bServerSendOne";
            this.bServerSendOne.Size = new System.Drawing.Size(75, 23);
            this.bServerSendOne.TabIndex = 12;
            this.bServerSendOne.Text = "Send One";
            this.bServerSendOne.UseVisualStyleBackColor = true;
            this.bServerSendOne.Click += new System.EventHandler(this.bServerSendOne_Click);
            // 
            // gbServer
            // 
            this.gbServer.Controls.Add(this.tbServerData);
            this.gbServer.Controls.Add(this.label1);
            this.gbServer.Controls.Add(this.bServerStart);
            this.gbServer.Controls.Add(this.bServerSendOne);
            this.gbServer.Controls.Add(this.bServerEnd);
            this.gbServer.Controls.Add(this.bServerSendAll);
            this.gbServer.Location = new System.Drawing.Point(12, 12);
            this.gbServer.Name = "gbServer";
            this.gbServer.Size = new System.Drawing.Size(337, 82);
            this.gbServer.TabIndex = 13;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "Server";
            // 
            // gbClient
            // 
            this.gbClient.Controls.Add(this.tbClientData);
            this.gbClient.Controls.Add(this.label2);
            this.gbClient.Controls.Add(this.bClientStart);
            this.gbClient.Controls.Add(this.bClientEnd);
            this.gbClient.Controls.Add(this.bClientSend);
            this.gbClient.Enabled = false;
            this.gbClient.Location = new System.Drawing.Point(12, 100);
            this.gbClient.Name = "gbClient";
            this.gbClient.Size = new System.Drawing.Size(337, 81);
            this.gbClient.TabIndex = 14;
            this.gbClient.TabStop = false;
            this.gbClient.Text = "Client";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Server Data Recieved";
            // 
            // tbServerData
            // 
            this.tbServerData.Location = new System.Drawing.Point(125, 49);
            this.tbServerData.Name = "tbServerData";
            this.tbServerData.ReadOnly = true;
            this.tbServerData.Size = new System.Drawing.Size(199, 20);
            this.tbServerData.TabIndex = 14;
            // 
            // tbClientData
            // 
            this.tbClientData.Location = new System.Drawing.Point(125, 48);
            this.tbClientData.Name = "tbClientData";
            this.tbClientData.ReadOnly = true;
            this.tbClientData.Size = new System.Drawing.Size(199, 20);
            this.tbClientData.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Client Data Recieved";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 192);
            this.Controls.Add(this.gbClient);
            this.Controls.Add(this.gbServer);
            this.Name = "Form1";
            this.Text = "Test Connection App";
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.gbClient.ResumeLayout(false);
            this.gbClient.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bServerStart;
        private System.Windows.Forms.Button bServerEnd;
        private System.Windows.Forms.Button bClientStart;
        private System.Windows.Forms.Button bClientEnd;
        private System.Windows.Forms.Button bServerSendAll;
        private System.Windows.Forms.Button bClientSend;
        private System.Windows.Forms.Button bServerSendOne;
        private System.Windows.Forms.GroupBox gbServer;
        private System.Windows.Forms.GroupBox gbClient;
        private System.Windows.Forms.TextBox tbServerData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbClientData;
        private System.Windows.Forms.Label label2;
    }
}

