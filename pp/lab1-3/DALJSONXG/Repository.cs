using GREPO;

namespace DALJSONXG
{
    public interface IRepository : GREPO.IRepository { }

    public class Repository : IRepository
    {
        JSONContext context;

        public Repository(string fileName)
        {
            context = JSONContext.Create(fileName);
        }

        public static IRepository Create() { return new Repository("WSRef.json"); }

        public List<Comment> getAllComment()
        {
            return context.Comments.ToList<Comment>();
        }

        public List<WSRef> getAllWSRef()
        {
            return context.WSRefs.ToList<WSRef>();
        }

        public bool addWSRef(WSRef wsRef)
        {
            bool rws = false;
            if (context.WSRefs != null && wsRef != null)
            {
                wsRef.Id = context.WSRefs.Any() ? context.WSRefs.Max(w => w.Id) + 1 : 1;
                context.WSRefs.Add(wsRef);
                rws = (context.SaveChanges() > 0);
            }
            return rws;
        }

        public bool addComment(Comment comment)
        {
            if (comment == null || context.WSRefs == null || context.WSRefs.Count == 0)
            {
                return false;
            }

            var wsref = context.WSRefs.FirstOrDefault(w => w.Id == comment.WSrefId);

            if (wsref == null)
            {
                return false;
            }

            if (wsref.Comments == null)
            {
                wsref.Comments = new List<Comment>();
            }

            wsref.Comments.Add(comment);
            context.SaveChanges();
            return true;
        }

        public Comment? GetCommentById(int Id)
        {
            return context.Comments.FirstOrDefault(c => c.Id == Id);
        }

        public void Dispose()
        {
        }
    }
}
