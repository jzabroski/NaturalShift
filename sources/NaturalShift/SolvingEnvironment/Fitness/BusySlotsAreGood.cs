﻿//-----------------------------------------------------------------------
// <copyright file="BusySlotsAreGood.cs" company="supix">
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
using System.Diagnostics;
using NaturalShift.SolvingEnvironment.Matrix;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class BusySlotsAreGood : IFitnessDimension
    {
        private readonly Single[] slotValues;
        private ShiftMatrix lastMatrix = null;
        private Single lastNormalizer;

        public BusySlotsAreGood(Single[] slotValues)
        {
            this.slotValues = slotValues;
        }

        public float Evaluate(ShiftMatrix matrix)
        {
            SetNormalizer(matrix);

            Single value = 0;
            for (int day = 0; day < matrix.Days; day++)
                for (int slot = 0; slot < matrix.Slots; slot++)
                {
                    var all = matrix[day, slot];
                    if ((!all.Forced) && (all.ChosenItem.HasValue))
                        value += (slotValues != null ? slotValues[slot] : 1);
                }

            var result = value / lastNormalizer;

            Debug.Assert(result >= 0 && result <= 1);

            return result;
        }

        private void SetNormalizer(ShiftMatrix matrix)
        {
            //set the normalizer the first time
            if ((lastMatrix == null) || !object.ReferenceEquals(matrix, lastMatrix))
            {
                lastMatrix = matrix;
                lastNormalizer = 0;
                for (int day = 0; day < matrix.Days; day++)
                    for (int slot = 0; slot < matrix.Slots; slot++)
                    {
                        var all = matrix[day, slot];
                        if (!all.Forced)
                            lastNormalizer += (slotValues != null ? slotValues[slot] : 1);
                    }
            }
        }
    }
}