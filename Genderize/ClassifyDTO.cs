namespace Genderize
{
    public class ClassifyDTO
    {
       
        public string Name { get; set; }
        public string Gender { get; set; }
        public double Probability { get; set; }
        public int Sample_size { get; set; }
        public bool Is_confident { get; set; }
        public DateTime Processed_at { get; set; }
    }
}
