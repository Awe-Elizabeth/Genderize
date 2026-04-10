namespace Genderize
{
    public class ClassifyDTO
    {
       
        public string Name { get; set; }
        public string Gender { get; set; }
        public double Probability { get; set; }
        public int Sample_Size { get; set; }
        public bool Is_Confident { get; set; }
        public DateTime Processed_At { get; set; }
    }
}
