ScanArea SAVE TO [scannedObjects]
[None] SAVE TO [storedObject]
DistanceBetween [Player] [storedObject] SAVE TO [stObjDis]

FOR SINGLE [object] FROM [scannedObjects] REPEAT
	DistanceBetween [Player] [object] SAVE TO [objDis]
	IF [object] IS [Rock] AND [objDis] LESS THAN [stObjDis] THEN
		[object] SAVE TO [storedObject]
		[objDis] SAVE TO [stObjDis]

IF [storedObject] IS [Rock] AND [stObjDis] LESS THAN RangeOf [Player] THEN 
	FOR SINGLE [item] FROM [Items] REPEAT
		IF [item] IS [Tool] THEN
			Use [item]
			FINISH