using Database.Entities;
namespace Services.Core.Contracts
{
    public class StatisticalResponse
    {
        public string[] labels {  get; set; }
        public int[] datasets { get; set; }
    }
}