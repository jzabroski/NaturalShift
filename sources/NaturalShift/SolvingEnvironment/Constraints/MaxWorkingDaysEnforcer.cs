﻿//-----------------------------------------------------------------------
// <copyright file="MaxWorkingDaysEnforcer.cs" company="supix">
//
// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
//
// This file is part of NaturalShift.
// NaturalShift is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// NaturalShift is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NaturalShift.SolvingEnvironment.Matrix;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class MaxWorkingDaysEnforcer : IConstraintEnforcer
    {
        private readonly int maxWorkingDays;
        private readonly int restAfterMaxWorkingDaysReached;

        public MaxWorkingDaysEnforcer(int maxWorkingDays, int restAfterMaxWorkingDaysReached)
        {
            this.maxWorkingDays = maxWorkingDays;
            this.restAfterMaxWorkingDaysReached = restAfterMaxWorkingDaysReached;
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            if (matrix[day, slot].ChosenItem.HasValue)
            {
                int item = matrix[day, slot].ChosenItem.Value;
                int workingDays = 1;

                {
                    var d = day - 1;
                    while ((d >= 0) && (workingDays < maxWorkingDays))
                    {
                        bool found = false;
                        for (int s = 0; s < matrix.Slots; s++)
                            if ((matrix[d, s].ChosenItem.HasValue) && (matrix[d, s].ChosenItem.Value == item))
                            {
                                found = true;
                                break;
                            }
                        if (found)
                            workingDays++;
                        else
                            break;

                        d--;
                    }
                }

                if (workingDays > maxWorkingDays)
                    throw new InvalidOperationException();

                {
                    if (workingDays == maxWorkingDays)
                    {
                        var untilDay = day + restAfterMaxWorkingDaysReached;
                        if (untilDay >= matrix.Days)
                            untilDay = matrix.Days - 1;

                        for (int d = day + 1; d <= untilDay; d++)
                            for (int s = 0; s < matrix.Slots; s++)
                                if (!matrix[d, s].Processed)
                                    matrix[d, s].CurrentAptitudes[item] = 0;
                    }
                }
            }
        }
    }
}