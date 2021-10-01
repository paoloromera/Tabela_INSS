using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INSS
{
    class TabelInss : ICalculadorInss
    {
        private DataTable _tabelaInss;
        private Dictionary<int, double> teto;

        public TabelInss()
        {
            teto = new Dictionary<int, double>();

            _tabelaInss = new DataTable();
            _tabelaInss.Columns.Add("valor_minimo", typeof(double));
            _tabelaInss.Columns.Add("valor_maximo", typeof(double));
            _tabelaInss.Columns.Add("aliquota", typeof(double));
            _tabelaInss.Columns.Add("ano", typeof(int));
            CarregarTabela();
        }

        public decimal CalcularDesconto(DateTime data, decimal salario)
        {
            decimal teto = Convert.ToDecimal(GetTeto(data.Year));
            _tabelaInss.DefaultView.RowFilter = string.Format("[valor_minimo] <= {0} and [valor_maximo] >= {0} and [ano] = {1}", salario, data.Year);
            DataTable resultado = _tabelaInss.DefaultView.ToTable();
            if (resultado.Rows.Count == 0)
                return teto;
            else
                return Math.Round((Convert.ToDecimal(resultado.Rows[0]["aliquota"]) / 100) * salario, 2);
        }

        private void AddAliquota(double pValMin, double pValMax, double pAliquota, int pAno)
        {
            DataRow dr = _tabelaInss.NewRow();
            dr["valor_minimo"] = pValMin;
            dr["valor_maximo"] = pValMax;
            dr["aliquota"] = pAliquota;
            dr["ano"] = pAno;
            _tabelaInss.Rows.Add(dr);
        }

        private double GetTeto(int ano)
        {
            return teto[ano];
        }

        /* PARA FINS DE TESTES */

        private void CarregarTabela()
        {
            AddAliquota(0, 1106.90, 8, 2011);
            AddAliquota(1106.91, 1844.83, 9, 2011);
            AddAliquota(1844.84, 3689.66, 11, 2011);
            teto.Add(2011, 405.86);

            AddAliquota(0, 1000, 7, 2012);
            AddAliquota(1000.01, 1500, 8, 2012);
            AddAliquota(1500.01, 3000, 9, 2012);
            AddAliquota(3000.01, 4000, 11, 2012);
            teto.Add(2012, 500);
        }
    }
}
