

using GestaoJogosEFCore.Data;
using GestaoJogosEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoJogosEFCore.Repositories
{
    public class JogoRepository
    {
        // Guarda a configuração do EF Core para criar contextos nas operações do respositorio
        private readonly DbContextOptions<GestaoJogosContext> _options;

        public JogoRepository(DbContextOptions<GestaoJogosContext> options)
        {
            // Rece as opções configuradas na inicialização da aplicação
            _options = options;

        }

        public List<Jogo> Listar()
        {
            // CADA OPERAÇÃO USA SEU PROPRIO CONTEXTO. USIG DESCARTA O CONTEXTO AO FINALIZAR O METODO
            using var context = new GestaoJogosContext(_options);

            return context.Jogo
            // Include carrega a navegação da desenvolvedora junto com os jogos
            .Include(j => j.IdDesenvolvedoraNavigation)
            // Ordena os dados dos jogos com base no ID
            .OrderBy(j => j.IdJogo)
            // Executa a consulta no banco de dados e devolve os dados em uma lista
            .ToList();
        }

        public Jogo? BuscarPorId(int id)
        {
             using var context = new GestaoJogosContext(_options);


             return context.Jogo
             .Include(j => j.IdDesenvolvedoraNavigation)
            //  Retorna o primeiro resultado ou o valor padrao = null
             .FirstOrDefault(j => j.IdJogo == id);
        }

        public void Adicionar(Jogo jogo)
        {
            using var context = new GestaoJogosContext(_options);

            context.Jogo.Add(jogo);
            context.SaveChanges(); //Como se fosse o commit no banco de dados  
        } 

        public bool Atualizar(Jogo jogoAtualizado)
        {
            using var context = new GestaoJogosContext(_options);

            // Find procura pela PK e rastreia  as entidades encontradas
            var jogo = context.Jogo.Find(jogoAtualizado.IdJogo);
            if (jogo is null)
            {
                return  false;
            }
            // copiando para a entidade encontradaos valores recebidos pelo objeto jogo atualizado
            jogo.IdDesenvolvedora = jogoAtualizado.IdDesenvolvedora;
            jogo.Nome = jogoAtualizado.Nome;
            jogo.Genero = jogoAtualizado.Genero;
            jogo.AnoLancamento = jogoAtualizado.AnoLancamento;
            jogo.Preco = jogoAtualizado.Preco;
            
            context.SaveChanges();

            return true;
            
        }

        public bool Excluir(int id)
        {
            using var context = new GestaoJogosContext(_options);

            var jogo = context.Jogo.Find(id);

            if (jogo is null)
            {
                return  false;
            }

            //Exclui o jogo como DELET faria pelo SQL
            context.Jogo.Remove(jogo);
            context.SaveChanges();
            return true;


        }

        public List<Desenvolvedora> ListarDesenvolvedoras()
        {
             using var context = new GestaoJogosContext(_options);

             return context.Desenvolvedora

             .OrderBy(d => d.Nome)
             .ToList();

        }

        public void AdicionarDesenvolvedora(Desenvolvedora  desenvolvedora)
        {

        using var context = new GestaoJogosContext(_options);

        context.Desenvolvedora.Add(desenvolvedora);
        context.SaveChanges();
        }
        
    }
}