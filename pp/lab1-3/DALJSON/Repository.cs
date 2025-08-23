using REPO;

namespace DALJSON
{
    public class Repository : IRepository
    {
        JSONContext context;

        private Repository()
        {
            context = JSONContext.Create("WSRef.json");
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
            bool rc = false;

            if (!(rc = (comment == null) || (context.WSRefs == null) || (context.WSRefs.Count == 0)))
            {
                var wsref = context.WSRefs?.Where(w => w.Id == comment?.WSrefId).FirstOrDefault();

                if (wsref != null)
                {
                    if (wsref.Comments == null)
                    {
                        wsref.Comments = new List<Comment>();
                    }    

                    wsref.Comments.Add(comment);
                    context.SaveChanges();
                    rc = true;
                }
                else
                {
                    rc = false;
                }
            }
            return rc;
        }

        public void Dispose()
        {
        }
    }
}
