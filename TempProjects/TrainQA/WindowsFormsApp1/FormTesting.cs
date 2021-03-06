﻿using DTLExpert.Models;
using System;

using System.IO;

using System.Windows.Forms;

namespace DTLExpert
{
    public partial class FormTesting : Form
    {
        public FormTesting()
        {
            InitializeComponent();
            inSizes = GetSizesData();
        }
        private  double[] inSizes;

        clsDTLAdvisor Advisor;
        private void btnTrain_Click(object sender, EventArgs e)
        {
            //Get sizes data
            clsJobTrain JobTrain = new clsJobTrain();
            JobTrain.inSizeSamples = inSizes;
            JobTrain.JobTrainProgressTick += jobTrainProgressTicked;

            JobTrain.Go(chkRecalcGains.Checked);




            MessageBox.Show("done...");
        }

        private double[] GetSizesData()
        {
            string[] lines = File.ReadAllLines("D:\\Projects\\DTL\\TempProjects\\TrainQA\\SizeDataShort.csv");
            double[] Sizes = new double[lines.Length - 1];
            int pos = -1;
            foreach (string line in lines)
            {
                if (pos == -1)
                {
                    pos++;
                    continue; //Skip header
                }


                char[] aCh = new char[1];
                aCh[0] = ',';
                string[] aValues = line.Split(aCh);
                double dSize = double.Parse(aValues[4]);
                 Sizes[pos] = dSize;
                pos++;
            }

            return Sizes;
        }

        private void jobTrainProgressTicked(int iCountDone, int iCountTotal)
        {
            int inewpbSize = 100 * iCountDone / iCountTotal;
            progressBar1.Value = inewpbSize;
            lblPB.Text = iCountDone.ToString() + "/" + iCountTotal.ToString();
            Application.DoEvents();


        }

        private void btnAdvise_Click(object sender, EventArgs e)
        {
            if ( Advisor==null)
                Advisor = new clsDTLAdvisor();
            int CurrSize = int.Parse(txtPosition.Text);
            int CurrDir = int.Parse(txtDir.Text);

            int AdvisedDir=-9 ;
            string AdvisedReturn="-9";
            string AdvisedAbort="-9";

            if (CurrDir != 0)
            {
                PositionState CurrPositionState = new PositionState(CurrDir, CurrSize,-9,-9,-9);
                FromPositionToAdvice _FromPositionToAdvice = Advisor.AdviceFromPositionTo(CurrPositionState);
                if (_FromPositionToAdvice is FromPositionToHoldAdvice)
                {
                    AdvisedDir = _FromPositionToAdvice.dir;
                    AdvisedReturn = ((FromPositionToHoldAdvice)_FromPositionToAdvice).returnn.ToString();
                    AdvisedAbort = ((FromPositionToHoldAdvice)_FromPositionToAdvice).abort.ToString();
                }

                if (_FromPositionToAdvice is FromPositionToAbortAdvice)
                {
                    AdvisedDir = _FromPositionToAdvice.dir;
                 }


            }


            if (CurrDir == 0)
            {
                OrbitState CurrOrbitState = new OrbitState( CurrSize,-9);
                FromOrbitToAdvice _FromOrbitToAdvice = Advisor.AdviceFromOrbitTo(CurrOrbitState);
                if (_FromOrbitToAdvice is FromOrbitToPoitionAdvice)
                {
                    AdvisedDir = _FromOrbitToAdvice.dir;
                    AdvisedReturn = ((FromOrbitToPoitionAdvice)_FromOrbitToAdvice).returnn.ToString();
                    AdvisedAbort = ((FromOrbitToPoitionAdvice)_FromOrbitToAdvice).abort.ToString();
                }

                if (_FromOrbitToAdvice is FromOrbitToWaitAdvice)
                {
                    AdvisedDir = _FromOrbitToAdvice.dir;
                }


            }



            txtAdvisedDir.Text = AdvisedDir.ToString();
            txtAdvisedReturn.Text = AdvisedReturn;
            txtAdvisedAbort.Text = AdvisedAbort;
    
   

        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            clsJobEvaluation JobEvaluation = new clsJobEvaluation();
            JobEvaluation.inSizes = inSizes;
             JobEvaluation.Go();




            MessageBox.Show("done...");

        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            clsJobLoadData JobLoadData = new clsJobLoadData();
            JobLoadData.Go();




            MessageBox.Show("done...");

        }
    }
 
}
