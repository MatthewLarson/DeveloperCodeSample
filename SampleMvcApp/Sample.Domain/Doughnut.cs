using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain
{
    public class Doughnut
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Flavor")]
        public int FlavorId { get; set; }
        public virtual Flavor Flavor { get; set; }
    }

    public class Flavor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Doughnut> Doughnuts { get; set; }
    }
    
}
