using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AiConversationDAO
    {
        public static async Task CreateConversation(Aiconversation conversation)
        {
            try
            {
                var _context = new HealthCareSystemContext();
                await _context.Aiconversations.AddAsync(conversation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<Aiconversation> GetConversationByUserId(int userId)
        {
            try
            {
                var _context = new HealthCareSystemContext();
                return await _context.Aiconversations.FirstOrDefaultAsync(c => c.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
