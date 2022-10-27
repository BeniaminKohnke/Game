ScanArea SAVE TO [scannedObjects]
[None] SAVE TO [storedObject]
DistanceBetween [Player] [storedObject] SAVE TO [stObjDis]

FOR SINGLE [sObject] FROM [scannedObjects] REPEAT
	DistanceBetween [Player] [sObject] SAVE TO [objDis]
	IF [sObject] IS [Rock] AND [objDis] LESS THAN [stObjDis] THEN
		[sObject] SAVE TO [storedObject]
		[sObject] SAVE TO [stObjDis]

IF [storedObject] IS [Rock] AND [stObjDis] LESS THAN RangeOf [Player] THEN 
	FOR SINGLE [item] FROM [Items] REPEAT
		IF [item] IS [Mele] THEN
			Use [item]
			FINISH