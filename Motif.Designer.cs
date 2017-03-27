namespace ClassificationMotif
{
    partial class Motif
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartLine = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.runBtn = new System.Windows.Forms.Button();
            this.browseBtn = new System.Windows.Forms.Button();
            this.SdwTxt = new System.Windows.Forms.TextBox();
            this.RTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartLine)).BeginInit();
            this.SuspendLayout();
            // 
            // chartLine
            // 
            chartArea1.Name = "ChartArea1";
            this.chartLine.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartLine.Legends.Add(legend1);
            this.chartLine.Location = new System.Drawing.Point(2, 2);
            this.chartLine.Name = "chartLine";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "rawData";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "motif";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "motifElement";
            this.chartLine.Series.Add(series1);
            this.chartLine.Series.Add(series2);
            this.chartLine.Series.Add(series3);
            this.chartLine.Size = new System.Drawing.Size(1481, 681);
            this.chartLine.TabIndex = 0;
            this.chartLine.Text = "chart1";
            // 
            // runBtn
            // 
            this.runBtn.Location = new System.Drawing.Point(1408, 689);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(75, 23);
            this.runBtn.TabIndex = 1;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(1327, 689);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(75, 23);
            this.browseBtn.TabIndex = 2;
            this.browseBtn.Text = "Browse..";
            this.browseBtn.UseVisualStyleBackColor = true;
            this.browseBtn.Click += new System.EventHandler(this.browseBtn_Click);
            // 
            // SdwTxt
            // 
            this.SdwTxt.Location = new System.Drawing.Point(1190, 690);
            this.SdwTxt.Name = "SdwTxt";
            this.SdwTxt.Size = new System.Drawing.Size(100, 22);
            this.SdwTxt.TabIndex = 3;
            // 
            // RTxt
            // 
            this.RTxt.Location = new System.Drawing.Point(1023, 690);
            this.RTxt.Name = "RTxt";
            this.RTxt.Size = new System.Drawing.Size(100, 22);
            this.RTxt.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(999, 693);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "R";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1144, 692);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "SDW";
            // 
            // Motif
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1514, 719);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RTxt);
            this.Controls.Add(this.SdwTxt);
            this.Controls.Add(this.browseBtn);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.chartLine);
            this.Name = "Motif";
            this.Text = "Motif Discovery";
            ((System.ComponentModel.ISupportInitialize)(this.chartLine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartLine;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.TextBox SdwTxt;
        private System.Windows.Forms.TextBox RTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

