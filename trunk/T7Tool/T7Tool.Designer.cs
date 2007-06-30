namespace T7Tool
{
    partial class T7Tool
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patchTabControl = new System.Windows.Forms.TabControl();
            this.fileInfoPage = new System.Windows.Forms.TabPage();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.saveFileAsbButton = new System.Windows.Forms.Button();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.openFileButton = new System.Windows.Forms.Button();
            this.fixChecksumButton = new System.Windows.Forms.Button();
            this.getIDButton = new System.Windows.Forms.Button();
            this.testLabel = new System.Windows.Forms.Label();
            this.fileInfoTable = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chassisIDTextBox = new System.Windows.Forms.TextBox();
            this.imobilizerLabel = new System.Windows.Forms.Label();
            this.checksumFBTextBox = new System.Windows.Forms.TextBox();
            this.asdfasdf = new System.Windows.Forms.Label();
            this.imobilizerTextBox = new System.Windows.Forms.TextBox();
            this.checksumF2TextBox = new System.Windows.Forms.TextBox();
            this.checksumFWTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.firmwareLengthTextBox = new System.Windows.Forms.TextBox();
            this.softwareVersionTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.carDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.chassisID = new System.Windows.Forms.Label();
            this.createPatchTabPage = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.loadOriginalFileButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.applyPatchTabPage = new System.Windows.Forms.TabPage();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.getIDFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.patchTabControl.SuspendLayout();
            this.fileInfoPage.SuspendLayout();
            this.fileInfoTable.SuspendLayout();
            this.createPatchTabPage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(436, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // patchTabControl
            // 
            this.patchTabControl.Controls.Add(this.fileInfoPage);
            this.patchTabControl.Controls.Add(this.createPatchTabPage);
            this.patchTabControl.Controls.Add(this.applyPatchTabPage);
            this.patchTabControl.Location = new System.Drawing.Point(0, 27);
            this.patchTabControl.Name = "patchTabControl";
            this.patchTabControl.SelectedIndex = 0;
            this.patchTabControl.Size = new System.Drawing.Size(437, 377);
            this.patchTabControl.TabIndex = 1;
            // 
            // fileInfoPage
            // 
            this.fileInfoPage.Controls.Add(this.fileNameLabel);
            this.fileInfoPage.Controls.Add(this.saveFileAsbButton);
            this.fileInfoPage.Controls.Add(this.saveFileButton);
            this.fileInfoPage.Controls.Add(this.openFileButton);
            this.fileInfoPage.Controls.Add(this.fixChecksumButton);
            this.fileInfoPage.Controls.Add(this.getIDButton);
            this.fileInfoPage.Controls.Add(this.testLabel);
            this.fileInfoPage.Controls.Add(this.fileInfoTable);
            this.fileInfoPage.Controls.Add(this.chassisID);
            this.fileInfoPage.Location = new System.Drawing.Point(4, 22);
            this.fileInfoPage.Name = "fileInfoPage";
            this.fileInfoPage.Padding = new System.Windows.Forms.Padding(3);
            this.fileInfoPage.Size = new System.Drawing.Size(429, 351);
            this.fileInfoPage.TabIndex = 0;
            this.fileInfoPage.Text = "File info";
            this.fileInfoPage.UseVisualStyleBackColor = true;
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(51, 239);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(0, 13);
            this.fileNameLabel.TabIndex = 10;
            // 
            // saveFileAsbButton
            // 
            this.saveFileAsbButton.Location = new System.Drawing.Point(309, 296);
            this.saveFileAsbButton.Name = "saveFileAsbButton";
            this.saveFileAsbButton.Size = new System.Drawing.Size(75, 23);
            this.saveFileAsbButton.TabIndex = 9;
            this.saveFileAsbButton.Text = "Save as...";
            this.saveFileAsbButton.UseVisualStyleBackColor = true;
            this.saveFileAsbButton.Click += new System.EventHandler(this.saveFileAsbButton_Click);
            // 
            // saveFileButton
            // 
            this.saveFileButton.Location = new System.Drawing.Point(207, 296);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(75, 23);
            this.saveFileButton.TabIndex = 8;
            this.saveFileButton.Text = "Save";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(100, 296);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(75, 23);
            this.openFileButton.TabIndex = 7;
            this.openFileButton.Text = "Open file";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // fixChecksumButton
            // 
            this.fixChecksumButton.Location = new System.Drawing.Point(288, 68);
            this.fixChecksumButton.Name = "fixChecksumButton";
            this.fixChecksumButton.Size = new System.Drawing.Size(94, 23);
            this.fixChecksumButton.TabIndex = 6;
            this.fixChecksumButton.Text = "Fix checksums";
            this.ToolTip.SetToolTip(this.fixChecksumButton, "Correct checksums");
            this.fixChecksumButton.UseVisualStyleBackColor = true;
            // 
            // getIDButton
            // 
            this.getIDButton.Location = new System.Drawing.Point(288, 16);
            this.getIDButton.Name = "getIDButton";
            this.getIDButton.Size = new System.Drawing.Size(94, 23);
            this.getIDButton.TabIndex = 5;
            this.getIDButton.Text = "Get ID";
            this.ToolTip.SetToolTip(this.getIDButton, "Get VIN and Immobilizer ID from another file. \r\nFor example from the original fil" +
                    "e for your car.");
            this.getIDButton.UseVisualStyleBackColor = true;
            this.getIDButton.Click += new System.EventHandler(this.getIDButton_Click);
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Location = new System.Drawing.Point(20, 239);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(26, 13);
            this.testLabel.TabIndex = 4;
            this.testLabel.Text = "File:";
            // 
            // fileInfoTable
            // 
            this.fileInfoTable.ColumnCount = 2;
            this.fileInfoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.fileInfoTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.fileInfoTable.Controls.Add(this.label1, 0, 3);
            this.fileInfoTable.Controls.Add(this.label2, 0, 2);
            this.fileInfoTable.Controls.Add(this.chassisIDTextBox, 1, 0);
            this.fileInfoTable.Controls.Add(this.imobilizerLabel, 0, 1);
            this.fileInfoTable.Controls.Add(this.checksumFBTextBox, 1, 3);
            this.fileInfoTable.Controls.Add(this.asdfasdf, 0, 0);
            this.fileInfoTable.Controls.Add(this.imobilizerTextBox, 1, 1);
            this.fileInfoTable.Controls.Add(this.checksumF2TextBox, 1, 2);
            this.fileInfoTable.Controls.Add(this.checksumFWTextBox, 1, 6);
            this.fileInfoTable.Controls.Add(this.label3, 0, 6);
            this.fileInfoTable.Controls.Add(this.label4, 0, 7);
            this.fileInfoTable.Controls.Add(this.firmwareLengthTextBox, 1, 7);
            this.fileInfoTable.Controls.Add(this.softwareVersionTextBox, 1, 8);
            this.fileInfoTable.Controls.Add(this.label5, 0, 8);
            this.fileInfoTable.Controls.Add(this.label6, 0, 9);
            this.fileInfoTable.Controls.Add(this.carDescriptionTextBox, 1, 9);
            this.fileInfoTable.Location = new System.Drawing.Point(17, 15);
            this.fileInfoTable.Name = "fileInfoTable";
            this.fileInfoTable.RowCount = 10;
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.fileInfoTable.Size = new System.Drawing.Size(265, 212);
            this.fileInfoTable.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Checksum FB";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Checksum F2";
            // 
            // chassisIDTextBox
            // 
            this.chassisIDTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chassisIDTextBox.Location = new System.Drawing.Point(135, 3);
            this.chassisIDTextBox.MaxLength = 30;
            this.chassisIDTextBox.Name = "chassisIDTextBox";
            this.chassisIDTextBox.Size = new System.Drawing.Size(127, 20);
            this.chassisIDTextBox.TabIndex = 5;
            this.chassisIDTextBox.TextChanged += new System.EventHandler(this.chassisIDTextBox_TextChanged);
            // 
            // imobilizerLabel
            // 
            this.imobilizerLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.imobilizerLabel.AutoSize = true;
            this.imobilizerLabel.Location = new System.Drawing.Point(3, 32);
            this.imobilizerLabel.Name = "imobilizerLabel";
            this.imobilizerLabel.Size = new System.Drawing.Size(72, 13);
            this.imobilizerLabel.TabIndex = 2;
            this.imobilizerLabel.Text = "Immobilizer ID";
            // 
            // checksumFBTextBox
            // 
            this.checksumFBTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checksumFBTextBox.Location = new System.Drawing.Point(135, 81);
            this.checksumFBTextBox.MaxLength = 8;
            this.checksumFBTextBox.Name = "checksumFBTextBox";
            this.checksumFBTextBox.ReadOnly = true;
            this.checksumFBTextBox.Size = new System.Drawing.Size(127, 20);
            this.checksumFBTextBox.TabIndex = 6;
            // 
            // asdfasdf
            // 
            this.asdfasdf.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.asdfasdf.AutoSize = true;
            this.asdfasdf.Location = new System.Drawing.Point(3, 6);
            this.asdfasdf.Name = "asdfasdf";
            this.asdfasdf.Size = new System.Drawing.Size(57, 13);
            this.asdfasdf.TabIndex = 7;
            this.asdfasdf.Text = "Chassis ID";
            // 
            // imobilizerTextBox
            // 
            this.imobilizerTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.imobilizerTextBox.Location = new System.Drawing.Point(135, 29);
            this.imobilizerTextBox.MaxLength = 15;
            this.imobilizerTextBox.Name = "imobilizerTextBox";
            this.imobilizerTextBox.Size = new System.Drawing.Size(127, 20);
            this.imobilizerTextBox.TabIndex = 8;
            this.imobilizerTextBox.TextChanged += new System.EventHandler(this.imobilizerTextBox_TextChanged);
            // 
            // checksumF2TextBox
            // 
            this.checksumF2TextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checksumF2TextBox.Location = new System.Drawing.Point(135, 55);
            this.checksumF2TextBox.MaxLength = 8;
            this.checksumF2TextBox.Name = "checksumF2TextBox";
            this.checksumF2TextBox.ReadOnly = true;
            this.checksumF2TextBox.Size = new System.Drawing.Size(127, 20);
            this.checksumF2TextBox.TabIndex = 9;
            // 
            // checksumFWTextBox
            // 
            this.checksumFWTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checksumFWTextBox.Location = new System.Drawing.Point(135, 107);
            this.checksumFWTextBox.MaxLength = 8;
            this.checksumFWTextBox.Name = "checksumFWTextBox";
            this.checksumFWTextBox.ReadOnly = true;
            this.checksumFWTextBox.Size = new System.Drawing.Size(127, 20);
            this.checksumFWTextBox.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Checksum FW";
            this.ToolTip.SetToolTip(this.label3, "Firmware checksum");
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Firmware length";
            // 
            // firmwareLengthTextBox
            // 
            this.firmwareLengthTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.firmwareLengthTextBox.Location = new System.Drawing.Point(135, 133);
            this.firmwareLengthTextBox.MaxLength = 8;
            this.firmwareLengthTextBox.Name = "firmwareLengthTextBox";
            this.firmwareLengthTextBox.ReadOnly = true;
            this.firmwareLengthTextBox.Size = new System.Drawing.Size(127, 20);
            this.firmwareLengthTextBox.TabIndex = 13;
            // 
            // softwareVersionTextBox
            // 
            this.softwareVersionTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.softwareVersionTextBox.Location = new System.Drawing.Point(135, 159);
            this.softwareVersionTextBox.MaxLength = 12;
            this.softwareVersionTextBox.Name = "softwareVersionTextBox";
            this.softwareVersionTextBox.Size = new System.Drawing.Size(127, 20);
            this.softwareVersionTextBox.TabIndex = 14;
            this.softwareVersionTextBox.TextChanged += new System.EventHandler(this.softwareVersionTextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Software version";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Car description";
            // 
            // carDescriptionTextBox
            // 
            this.carDescriptionTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.carDescriptionTextBox.Location = new System.Drawing.Point(135, 187);
            this.carDescriptionTextBox.MaxLength = 30;
            this.carDescriptionTextBox.Name = "carDescriptionTextBox";
            this.carDescriptionTextBox.Size = new System.Drawing.Size(127, 20);
            this.carDescriptionTextBox.TabIndex = 17;
            this.carDescriptionTextBox.TextChanged += new System.EventHandler(this.carDescriptionTextBox_TextChanged);
            // 
            // chassisID
            // 
            this.chassisID.AutoSize = true;
            this.chassisID.Location = new System.Drawing.Point(121, 31);
            this.chassisID.Name = "chassisID";
            this.chassisID.Size = new System.Drawing.Size(0, 13);
            this.chassisID.TabIndex = 1;
            // 
            // createPatchTabPage
            // 
            this.createPatchTabPage.Controls.Add(this.richTextBox1);
            this.createPatchTabPage.Controls.Add(this.tableLayoutPanel2);
            this.createPatchTabPage.Controls.Add(this.tableLayoutPanel1);
            this.createPatchTabPage.Location = new System.Drawing.Point(4, 22);
            this.createPatchTabPage.Name = "createPatchTabPage";
            this.createPatchTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.createPatchTabPage.Size = new System.Drawing.Size(429, 351);
            this.createPatchTabPage.TabIndex = 1;
            this.createPatchTabPage.Text = "Create patch";
            this.createPatchTabPage.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(8, 191);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(400, 96);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.28047F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.33055F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.38898F));
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.button2, 2, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 301);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(400, 37);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Patch file";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(269, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Create";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.02667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.54724F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.42609F));
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.loadOriginalFileButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 162);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Original file";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Tuned file";
            // 
            // loadOriginalFileButton
            // 
            this.loadOriginalFileButton.Location = new System.Drawing.Point(269, 3);
            this.loadOriginalFileButton.Name = "loadOriginalFileButton";
            this.loadOriginalFileButton.Size = new System.Drawing.Size(75, 23);
            this.loadOriginalFileButton.TabIndex = 3;
            this.loadOriginalFileButton.Text = "Load";
            this.loadOriginalFileButton.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(269, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Load";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Patch description";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 136);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Engine";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 104);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Tuner name";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "B204E",
            "B204R",
            "B234E",
            "B235R"});
            this.comboBox1.Location = new System.Drawing.Point(99, 127);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(99, 101);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(164, 20);
            this.textBox1.TabIndex = 10;
            // 
            // applyPatchTabPage
            // 
            this.applyPatchTabPage.Location = new System.Drawing.Point(4, 22);
            this.applyPatchTabPage.Name = "applyPatchTabPage";
            this.applyPatchTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.applyPatchTabPage.Size = new System.Drawing.Size(429, 351);
            this.applyPatchTabPage.TabIndex = 2;
            this.applyPatchTabPage.Text = "Apply patch";
            this.applyPatchTabPage.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "BIN";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk_1);
            // 
            // ToolTip
            // 
            this.ToolTip.ToolTipTitle = "Tool tip";
            // 
            // getIDFileDialog
            // 
            this.getIDFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.getIDFileDialog_FileOk);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.AddExtension = false;
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // T7Tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 402);
            this.Controls.Add(this.patchTabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "T7Tool";
            this.Text = "T7Tool";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.patchTabControl.ResumeLayout(false);
            this.fileInfoPage.ResumeLayout(false);
            this.fileInfoPage.PerformLayout();
            this.fileInfoTable.ResumeLayout(false);
            this.fileInfoTable.PerformLayout();
            this.createPatchTabPage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl patchTabControl;
        private System.Windows.Forms.TabPage fileInfoPage;
        private System.Windows.Forms.Label chassisID;
        private System.Windows.Forms.TableLayoutPanel fileInfoTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label imobilizerLabel;
        private System.Windows.Forms.TextBox chassisIDTextBox;
        private System.Windows.Forms.TextBox checksumFBTextBox;
        private System.Windows.Forms.Label asdfasdf;
        private System.Windows.Forms.TextBox imobilizerTextBox;
        private System.Windows.Forms.TextBox checksumF2TextBox;
        private System.Windows.Forms.TextBox checksumFWTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox firmwareLengthTextBox;
        private System.Windows.Forms.TextBox softwareVersionTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label testLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox carDescriptionTextBox;
        private System.Windows.Forms.Button fixChecksumButton;
        private System.Windows.Forms.Button getIDButton;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.TabPage createPatchTabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button loadOriginalFileButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage applyPatchTabPage;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Button saveFileAsbButton;
        private System.Windows.Forms.OpenFileDialog getIDFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

