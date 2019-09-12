namespace ConvertorToXml_v._1._0
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_Start = new System.Windows.Forms.Button();
            this.OpenExcel = new System.Windows.Forms.OpenFileDialog();
            this.YearUpDown = new System.Windows.Forms.NumericUpDown();
            this.MonthUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.YearUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // Button_Start
            // 
            this.Button_Start.Location = new System.Drawing.Point(47, 12);
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(75, 23);
            this.Button_Start.TabIndex = 0;
            this.Button_Start.Text = "Старт";
            this.Button_Start.UseVisualStyleBackColor = true;
            this.Button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // OpenExcel
            // 
            this.OpenExcel.FileName = "OpenExcel";
            // 
            // YearUpDown
            // 
            this.YearUpDown.Location = new System.Drawing.Point(180, 14);
            this.YearUpDown.Maximum = new decimal(new int[] {
            2030,
            0,
            0,
            0});
            this.YearUpDown.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.YearUpDown.Name = "YearUpDown";
            this.YearUpDown.Size = new System.Drawing.Size(46, 20);
            this.YearUpDown.TabIndex = 1;
            this.YearUpDown.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // MonthUpDown
            // 
            this.MonthUpDown.Location = new System.Drawing.Point(128, 14);
            this.MonthUpDown.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.MonthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MonthUpDown.Name = "MonthUpDown";
            this.MonthUpDown.Size = new System.Drawing.Size(46, 20);
            this.MonthUpDown.TabIndex = 2;
            this.MonthUpDown.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 54);
            this.Controls.Add(this.MonthUpDown);
            this.Controls.Add(this.YearUpDown);
            this.Controls.Add(this.Button_Start);
            this.Name = "Form1";
            this.Text = "ConvertorToXml";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.YearUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MonthUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.OpenFileDialog OpenExcel;
        private System.Windows.Forms.NumericUpDown YearUpDown;
        private System.Windows.Forms.NumericUpDown MonthUpDown;
    }
}

