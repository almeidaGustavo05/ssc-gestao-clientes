using GestaoClientes.Domain.Clientes;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace GestaoClientes.Infrastructure.Mappings
{
    public class ClienteMap : ClassMapping<Cliente>
    {
        public ClienteMap()
        {
            Table("Clientes");
            
            Id(x => x.Id, m => 
            {
                m.Generator(Generators.GuidComb);
                m.Column("Id");
            });

            Property(x => x.Nome, m => 
            { 
                m.Length(200); 
                m.NotNullable(true); 
            });

            Property(x => x.Email, m => 
            { 
                m.Length(200); 
                m.NotNullable(true); 
            });

            Property(x => x.Ativo, m => m.NotNullable(true));
            
            Property(x => x.DataCadastro, m => m.NotNullable(true));

            Component(x => x.Cnpj, m =>
            {
                m.Property(c => c.Valor, p => 
                { 
                    p.Column("Cnpj"); 
                    p.Length(14); 
                    p.NotNullable(true);
                    p.Unique(true);
                });
            });
        }
    }
}
