using GREPO;

namespace DALMSQLXG
{
    public interface IRepository : GREPO.IRepository {}

    public class Repository : IRepository
    {
         Context context;

        public Repository()
        {
            context = new Context();
        }

        public static IRepository Create() { return new Repository(); }

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
            if (context.WSRefs != null)
            {
                context.Add(wsRef);
                rws = (context.SaveChanges() > 0);
            }
            return rws;
        }

        public Comment? GetCommentById(int Id)
        {
            return context.Comments.FirstOrDefault(c => c.Id == Id);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public bool addComment(Comment comment)
        {
            if (comment == null || context.WSRefs == null) return false;

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
    }
}
