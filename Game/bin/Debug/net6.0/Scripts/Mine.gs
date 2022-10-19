ScanWorldFrom [Player] SAVE TO [scannedObjects]
[None] SAVE TO [storedObject]
DistanceBetween PositionOf [Player] PositionOf [storedObject] SAVE TO [stObjDis]

FOR SINGLE [object] FROM [scannedObjects] DO
	DistanceBetween PositionOf Player PositionOf [object] SAVE TO [objDis]
	IF [object] IS Rock AND [objDis] LESS THAN [stObjDis] THEN
		[object] SAVE TO [storedObject]
		[objDis] SAVE TO [stObjDis]

IF [storedObject] IS [Rock] AND [stObjDis] LESS THAN RangeOf [Player] THEN 
	FOR SINGLE [item] FROM [Items] DO
		IF TypeOf [item] IS [Tool] THEN
			Use [item]
			FINISH