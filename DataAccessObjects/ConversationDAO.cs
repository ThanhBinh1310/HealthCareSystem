using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessObjects
{
    public class ConversationDAO
    {
        private static ConversationDAO instance;
        private readonly HealthCareSystemContext _context;
        private static readonly object _lock = new object();

        public static ConversationDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new ConversationDAO();
                        }
                    }
                }
                return instance;
            }
        }

        public ConversationDAO()
        {
            _context = new HealthCareSystemContext();
        }

        // Get conversation by its ID
        public async Task<Conversation> GetConversationById(int conversationId)
        {
            try
            {
                return await _context.Conversations
                    .Include(c => c.PatientUser)
                    .Include(c => c.DoctorUser)
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Create a new conversation
        public async Task CreateConversation(Conversation conversation)
        {
            try
            {
                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Update an existing conversation
        public async Task UpdateConversation(Conversation conversation)
        {
            try
            {
                _context.Conversations.Update(conversation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Conversation>> GetConversationsByDoctorId(int doctorUserId)
        {
            try
            {
                // Fetch conversations where the Doctor is involved (either as DoctorUser or PatientUser)
                return await _context.Conversations
                    .Where(c => c.DoctorUserId == doctorUserId)  // Filter by DoctorUserId
                    .Include(c => c.PatientUser)  // Include PatientUser details
                    .Include(c => c.DoctorUser)  // Include DoctorUser details (just for completeness)
                    .Include(c => c.Messages)  // Include related messages
                    .ToListAsync();  // Return the list of conversations asynchronously
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving conversations for the doctor.", ex);
            }
        }
        public async Task<List<Conversation>> GetConversationsByPatientId(int patientUserId)
        {
            try
            {
                // Fetch conversations where the Patient is involved (either as DoctorUser or PatientUser)
                return await _context.Conversations
                    .Where(c => c.PatientUserId == patientUserId)  // Filter by PatientUserId
                    .Include(c => c.PatientUser)  // Include PatientUser details
                    .Include(c => c.DoctorUser)  // Include DoctorUser details (just for completeness)
                    .Include(c => c.Messages)  // Include related messages
                    .ToListAsync();  // Return the list of conversations asynchronously
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving conversations for the patient.", ex);
            }
        }


    }
}
