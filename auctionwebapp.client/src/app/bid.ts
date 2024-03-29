import { Lot } from './lot';
import { User } from './user';

export class Bid {
  constructor(
    public BId1?: number,
    public BTime?: Date,
    public BParticipantId?: number,
    public BSize?: number,
    public BLotId?: number,
    public BLot?: Lot,
    public BParticipant?: User
  ) { }
}
