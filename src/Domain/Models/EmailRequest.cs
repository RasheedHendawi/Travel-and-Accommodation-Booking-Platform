﻿using System.Net.Mail;

namespace Domain.Models
{
    public record EmailRequest(
      IEnumerable<string> ToEmails,
      string Subject,
      string Body,
      IEnumerable<AttachmentInvoice> Attachments);
}
