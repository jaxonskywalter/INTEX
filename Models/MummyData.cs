using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INTEX.Models
{
    public class MummyData
    {
        public float Depth { get; set; }
        public float Length { get; set; }
        public float headdirection_ { get; set; }
        public float headdirection_E { get; set; }
        public float headdirection_W { get; set; }
        public float sex_ { get; set; }
        public float sex_F { get; set; }
        public float sex_M { get; set; }
        public float area_NE { get; set; }
        public float area_SE { get; set; }
        public float area_SW { get; set; }
        public float ageatdeath_ { get; set; }
        public float ageatdeath_A { get; set; }
        public float ageatdeath_C { get; set; }
        public float ageatdeath_I { get; set; }
        public float ageatdeath_N { get; set; }


        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
            Depth,
            Length,
            headdirection_,
            headdirection_E,
            headdirection_W,
            sex_,
            sex_F,
            sex_M,
            area_NE,
            area_SE,
            area_SW,
            ageatdeath_,
            ageatdeath_A,
            ageatdeath_C,
            ageatdeath_I,
            ageatdeath_N,
            };
            int[] dimensions = new int[] { 1, 16 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
