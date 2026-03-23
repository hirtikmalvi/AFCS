import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hidecardnumber',
})
export class HideCardNumberPipe implements PipeTransform {
  transform(value: string): any {
    let cardNumber = value;
    return '******' + cardNumber.substring(cardNumber.length - 4);
  }
}
