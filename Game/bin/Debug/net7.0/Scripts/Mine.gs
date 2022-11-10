ScanArea SAVE TO [scannedObjects]
[None] SAVE TO [storedObject]
DistanceBetween [Player] [storedObject] SAVE TO [stObjDis]

FOR SINGLE [sObject] FROM [scannedObjects] DO
	DistanceBetween [Player] [sObject] SAVE TO [objDis]
	IF [sObject] IS [Rock] AND [objDis] LESS THAN [stObjDis] THEN
		[sObject] SAVE TO [storedObject]
		[objDis] SAVE TO [stObjDis]

IF [storedObject] IS [Rock] AND [stObjDis] LESS THAN [20] THEN 
	FOR SINGLE [item] FROM [Items] DO
		IF [item] IS ['Pickaxe'] THEN
			Use [item]
			FINISH
ELSE THEN
	GoTo DirectionBetween [Player] [storedObject]