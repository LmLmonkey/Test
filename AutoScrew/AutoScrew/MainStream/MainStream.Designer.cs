namespace AutoScrew
{
    partial class MainStream
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Station1Work = new System.ComponentModel.BackgroundWorker();
            this.Station2Work = new System.ComponentModel.BackgroundWorker();
            this.Station3Work = new System.ComponentModel.BackgroundWorker();
            this.backgroundSafeDoor = new System.ComponentModel.BackgroundWorker();
            this.backgroundCCD1Calibration = new System.ComponentModel.BackgroundWorker();
            this.backgroundCCD2Calibration = new System.ComponentModel.BackgroundWorker();
            this.backgroundCCD3Calibration = new System.ComponentModel.BackgroundWorker();
            this.backgroundCCD4Calibration = new System.ComponentModel.BackgroundWorker();
            this.backgroundReset = new System.ComponentModel.BackgroundWorker();
            this.Station4Worker = new System.ComponentModel.BackgroundWorker();
            // 
            // Station1Work
            // 
            this.Station1Work.WorkerReportsProgress = true;
            this.Station1Work.WorkerSupportsCancellation = true;
            this.Station1Work.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Station1Work_DoWork);
            // 
            // Station2Work
            // 
            this.Station2Work.WorkerReportsProgress = true;
            this.Station2Work.WorkerSupportsCancellation = true;
            this.Station2Work.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Station2Work_DoWork);
            // 
            // Station3Work
            // 
            this.Station3Work.WorkerReportsProgress = true;
            this.Station3Work.WorkerSupportsCancellation = true;
            this.Station3Work.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Station3Work_DoWork);
            // 
            // backgroundSafeDoor
            // 
            this.backgroundSafeDoor.WorkerReportsProgress = true;
            this.backgroundSafeDoor.WorkerSupportsCancellation = true;
            this.backgroundSafeDoor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundsafeDoor_DoWork);
            // 
            // backgroundCCD1Calibration
            // 
            this.backgroundCCD1Calibration.WorkerReportsProgress = true;
            this.backgroundCCD1Calibration.WorkerSupportsCancellation = true;
            this.backgroundCCD1Calibration.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundCCD1Calibration_DoWork);
            // 
            // backgroundCCD2Calibration
            // 
            this.backgroundCCD2Calibration.WorkerReportsProgress = true;
            this.backgroundCCD2Calibration.WorkerSupportsCancellation = true;
            this.backgroundCCD2Calibration.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundCCD2Calibration_DoWork);
            // 
            // backgroundCCD3Calibration
            // 
            this.backgroundCCD3Calibration.WorkerReportsProgress = true;
            this.backgroundCCD3Calibration.WorkerSupportsCancellation = true;
            this.backgroundCCD3Calibration.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundCCD3Calibration_DoWork);
            // 
            // backgroundCCD4Calibration
            // 
            this.backgroundCCD4Calibration.WorkerReportsProgress = true;
            this.backgroundCCD4Calibration.WorkerSupportsCancellation = true;
            this.backgroundCCD4Calibration.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundCCD4Calibration_DoWork);
            // 
            // backgroundReset
            // 
            this.backgroundReset.WorkerReportsProgress = true;
            this.backgroundReset.WorkerSupportsCancellation = true;
            // 
            // Station4Worker
            // 
            this.Station4Worker.WorkerReportsProgress = true;
            this.Station4Worker.WorkerSupportsCancellation = true;
            this.Station4Worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Station4Worker_DoWork);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker Station1Work;
        private System.ComponentModel.BackgroundWorker Station2Work;
        private System.ComponentModel.BackgroundWorker Station3Work;
        private System.ComponentModel.BackgroundWorker backgroundSafeDoor;
        private System.ComponentModel.BackgroundWorker backgroundCCD1Calibration;
        private System.ComponentModel.BackgroundWorker backgroundCCD2Calibration;
        private System.ComponentModel.BackgroundWorker backgroundCCD3Calibration;
        private System.ComponentModel.BackgroundWorker backgroundCCD4Calibration;
        private System.ComponentModel.BackgroundWorker backgroundReset;
        private System.ComponentModel.BackgroundWorker Station4Worker;
    }
}
