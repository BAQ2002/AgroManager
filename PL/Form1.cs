using INFRA;

namespace PL
{
    public partial class Form1 : Form
    {
        private readonly AgroManagerDbContext _dbContext;

        public Form1(AgroManagerDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            dataGridView1.DataSource = _dbContext;
        }
    }

}
