using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessObjects
{
    public class MessageDAO
    {
        private static MessageDAO instance;
        private readonly HealthCareSystemContext _context;
        private static readonly object _lock = new object();

        public static MessageDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new MessageDAO();
                        }
                    }
                }
                return instance;
            }
        }

        public MessageDAO()
        {
            _context = new HealthCareSystemContext();
        }

        // Get all messages for a specific conversation
        public async Task<List<Message>> GetMessagesByConversationId(int conversationId)
        {
            try
            {
                return await _context.Messages
                    .Where(m => m.ConversationId == conversationId)
                    .Include(m => m.Sender)
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Get a message by its ID
        public async Task<Message> GetMessageById(int messageId)
        {
            try
            {
                return await _context.Messages
                    .Include(m => m.Sender)
                    .FirstOrDefaultAsync(m => m.MessageId == messageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Create a new message
        public async Task CreateMessage(Message message)
        {
            try
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Mark a message as read
        public async Task MarkMessageAsRead(int messageId)
        {
            try
            {
                var message = await _context.Messages
                    .FirstOrDefaultAsync(m => m.MessageId == messageId);

                if (message != null)
                {
                    message.IsRead = true;
                    message.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Update message content
        public async Task UpdateMessage(Message message)
        {
            try
            {
                _context.Messages.Update(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
