﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NeptunLight.Models;
using NeptunLight.Services;

namespace NeptunLight.DataAccess
{
    public interface INeptunInterface
    {
        Task LoginAsync();

        Task<IReadOnlyCollection<Mail>> RefreshMessagesAsnyc(IMailContentCache contentCache = null);
        Task<IReadOnlyCollection<CalendarEvent>> RefreshCalendarAsnyc();
        Task<IReadOnlyDictionary<Semester, Subject>> RefreshSubjectsAsnyc();
        Task<IReadOnlyDictionary<Semester, Exam>> RefreshExamsAsnyc();
        Task<IReadOnlyCollection<SemesterData>> RefreshSemestersAsnyc();
        Task<IReadOnlyCollection<SemesterData>> RefreshPeriodsAsnyc();
    }
}