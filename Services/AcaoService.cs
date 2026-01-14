using InvestSite.API.Models;

namespace InvestSite.API.Services;

public class AcaoService
{
    public double PrecoTetoBazin(double dividendos)
    {
        return dividendos * 16.67;
    }

    public double PrecoTetoGraham(double lpa, double vpa)
    {
        return Math.Sqrt(22.5 * lpa * vpa);
    }
}
 