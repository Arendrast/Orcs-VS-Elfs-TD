using System;
using NUnit.Framework;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Tests
{
    public class MergeTests
    {
        [Test]
        public void WhenMergeTwoCells_AndTargetUnitsIdsAre0And1_ThenUnitsAreSwapped()
        {
            // Arrange.
            var mergeGridModel = Setup.MergeGrid(new int[] {0, 1});

            var firstCell = mergeGridModel.Cells[0];
            var secondCell = mergeGridModel.Cells[1];

            var firstCellUnit = firstCell.TargetUnit;
            var secondCellUnit = secondCell.TargetUnit;
            
            // Act.
            if (!mergeGridModel.TryMergeCells(firstCell, secondCell, out var didMoveTargetUnit))
            {
                throw new Exception("Cant merge grid cells");
            }

            // Assert.
            Assert.AreEqual(true,
                firstCell.TargetUnit == secondCellUnit &&
                secondCell.TargetUnit == firstCellUnit);
        }
        
        [Test]
        public void WhenMergeTwoCells_AndTargetUnitsIdsAre0And0_Then1CellShouldHaveNullTargetUnitAnd2CellShouldHaveUpgradedTargetUnit()
        {
            // Arrange.
            var mergeGridModel = Setup.MergeGrid(new int[] {0, 0});

            var firstCell = mergeGridModel.Cells[0];
            var secondCell = mergeGridModel.Cells[1];
            
            // Act.
            if (!mergeGridModel.TryMergeCells(firstCell, secondCell, out var didMoveTargetUnit))
            {
                throw new Exception("Cant merge grid cells");
            }
            
            var firstCellUnit = firstCell.TargetUnit;
            var secondCellUnit = secondCell.TargetUnit;

            // Assert.
            Assert.AreEqual(true, firstCellUnit == null && secondCellUnit.Id == 1);
        }
        
        [Test]
        public void WhenMergeTwoCells_And1Cell0HasTargetUnitId0And2CellHasNotTargetUnit_Then1Cell0HasNotTargetUnitAnd2CellHasPast1CellTargetUnit()
        {
            // Arrange.
            var mergeGridModel = Setup.MergeGrid(new int[] {0});

            var firstCell = mergeGridModel.Cells[0];
            var secondCell = mergeGridModel.Cells[1];
            
            var pastFirstCellUnit = firstCell.TargetUnit;
            
            // Act.
            if (!mergeGridModel.TryMergeCells(firstCell, secondCell, out var didMoveTargetUnit))
            {
                throw new Exception("Cant merge grid cells");
            }
            
            var firstCellUnit = firstCell.TargetUnit;
            var secondCellUnit = secondCell.TargetUnit;

            // Assert.
            Assert.AreEqual(true, firstCellUnit == null && secondCellUnit == pastFirstCellUnit);
        }
    }
}